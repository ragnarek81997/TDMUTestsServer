using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using TDMUTestsServer.Domain.Entities.Dictionaries;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Struct;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TDMUTestsServer.Infrastructure.Data.Utility.AzureBlob
{
    public class UploadBase
    {
        //public string PathFile
        //{
        //    get { return pathFile; }
        //}

        protected UploadBaseResult uploadBaseResult = new UploadBaseResult();
        protected string containerName;
        protected int maxLengthFile;
        protected string error;

        protected CloudBlobContainer container;

        //protected static Logger logger = LogManager.GetCurrentClassLogger();

        private static string cacheControl = "public, max-age=7776000";

        public UploadBase(string containerName)
        {
            if (string.IsNullOrEmpty(containerName))
                new ArgumentNullException(containerName);
            this.containerName = containerName;

            // Receiving Account storage 
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());

            // Create Blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // To get conteiner reference 
            container = blobClient.GetContainerReference(this.containerName);
        }

        //TODO: implement init container
        public static void Initialize()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"].ToString());

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            foreach (ContainerBlob item in Enum.GetValues(typeof(ContainerBlob)))
            {
                var container = blobClient.GetContainerReference(item.ToString());

                if (!container.Exists())
                {
                    container.CreateIfNotExists();
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
                }
            }

        }

        protected async Task<UploadBaseResult> UploadFileToAzureAsync(HttpPostedFileBase inputFile, string path, bool replace = false/*, bool download = false*/)
        {
            // To get blob object reference 
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path);

            try
            {
                // Do not rewrite the file 
                if (replace && blockBlob.Exists())
                {
                    uploadBaseResult.Error = ErrorDescriptions.FileExists;
                    return uploadBaseResult;
                }

                // To create BLOB object 
                var fileStream = inputFile.InputStream;

                // Indicating file type 
                blockBlob.Properties.ContentType = inputFile.ContentType;

                //if (download)
                //    blockBlob.Properties.ContentDisposition = "attachment";

                //add  CacheControl
                blockBlob.Properties.CacheControl = cacheControl;

                // Stream and save 
                await blockBlob.UploadFromStreamAsync(fileStream);

                // Save local path to Blob - object 
                uploadBaseResult.PathFile = blockBlob.Uri.LocalPath;

                return uploadBaseResult;
            }
            catch (Exception e)
            {
                uploadBaseResult.Error = e.Message.ToString();

                return uploadBaseResult;
            }
        }

        protected async Task<UploadBaseResult> UploadFileToAzureAsync(Stream inputStream, string path, string contentType, bool replace = false)
        {
            // Get reference on Blobk - object 
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(path);

            try
            {
                // Don't replace the file 
                if (replace && blockBlob.Exists())
                {
                    uploadBaseResult.Error = ErrorDescriptions.FileExists;
                    return uploadBaseResult;
                }

                blockBlob.Properties.ContentType = contentType;
                blockBlob.Properties.CacheControl = cacheControl;

                // Uploading stream and saving 
                await blockBlob.UploadFromStreamAsync(inputStream);

                // Save local path to BLOB - object 
                uploadBaseResult.PathFile = blockBlob.Uri.LocalPath;

                return uploadBaseResult;
            }
            catch (Exception e)
            {
                uploadBaseResult.Error = e.Message.ToString();

                return uploadBaseResult;
            }
        }

        public async Task<MemoryStream> DownloadFileToStreamAsync(string blobPapth)
        {
            // Get reference on BLOB - object 
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobPapth);

            try
            {
                var inputStream = new MemoryStream();

                // Download file to stream and return stream 
                await blockBlob.DownloadToStreamAsync(inputStream);

                return inputStream;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> DeleteFileFromAzureAsync(string path)
        {
            try
            {
                string replaceConteiner = string.Format("/{0}/", containerName);
                path = path.Replace(replaceConteiner, string.Empty);

                // To get blob object reference 
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(path);
                await blockBlob.DeleteIfExistsAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> DeleteFileFromAzure(string[] masPath)
        {
            int count = 0;

            foreach (var item in masPath)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    bool success = await this.DeleteFileFromAzureAsync(item);
                    if (success)
                        count++;
                }
            }

            return count;
        }

        public UploadBaseResult MoveFile(string inputPath, string sourcePath, bool newRandomName = false, string newName = null)
        {
            string nameMoveFile = string.Empty;

            if (newName != null)
            {
                string typeFile = inputPath.Substring(inputPath.LastIndexOf("."));
                nameMoveFile = newName + typeFile;
            }
            else if (newRandomName)
            {
                string typeFile = inputPath.Substring(inputPath.LastIndexOf("."));
                string tmpName = System.Guid.NewGuid().ToString("N").Substring(0, 10);
                nameMoveFile = tmpName + typeFile;
            }
            else
            {
                nameMoveFile = inputPath.Substring(inputPath.LastIndexOf("/") + 1);
            }

            //TODO: implement enum for name container
            inputPath = inputPath.Replace($"/{containerName}/", "");

            sourcePath = sourcePath + nameMoveFile;

            // Get reference to existant BLOB-object 
            CloudBlockBlob blockBlobExists = container.GetBlockBlobReference(inputPath);

            // Get reference to new BLOB - object 
            CloudBlockBlob blockBlobNew = container.GetBlockBlobReference(sourcePath);

            try
            {
                // Copy Blob 
                blockBlobNew.StartCopy(blockBlobExists);

                // Name of copied file 
                uploadBaseResult.PathFile = blockBlobNew.Uri.LocalPath;
                blockBlobExists.Delete();

                return uploadBaseResult;
            }
            catch (Exception e)
            {
                uploadBaseResult.Error = e.Message.ToString();

                return uploadBaseResult;
            }
        }

    }
}
