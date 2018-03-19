using TDMUTestsServer.Domain.Entities.Dictionaries;
using TDMUTestsServer.Domain.Entities.Enums;
using TDMUTestsServer.Domain.Entities.Struct;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TDMUTestsServer.Infrastructure.Data.Utility.AzureBlob
{
    public class UploadImage : UploadBase
    {
        private UploadBaseResult uploadImageResult = new UploadBaseResult();

        public UploadImage() : base(ContainerBlob.Images.ToString().ToLower())
        {
            maxLengthFile = 20000;
        }


        /*-----------------------------------------------------------------*/

        public async Task<UploadBaseResult> ImageCropAndResize(HttpPostedFile inputFile, string path = null, int w=0, int h=0)
        {
            try
            {
                Image image = Image.FromStream(inputFile.InputStream, true, true);
                SetImageOrientation(ref image);

                string nameFile;

                // If path null save to temporary directory 
                if (path == null)
                    path = UploadImageProperties.TemporaryFolder;

                if (inputFile == null)
                {
                    uploadImageResult.Error = ErrorDescriptions.FileMissing;

                    return uploadImageResult;
                }

                // Obtaining Content Type 
                string fileType = SupportMimeType(inputFile.ContentType);

                // Checking file for errors 
                bool checkFIle = CheckFile(inputFile.InputStream, fileType);

                if (!checkFIle)
                {
                    uploadImageResult.Error += error;

                    return uploadImageResult;
                }

                nameFile = System.Guid.NewGuid().ToString("N").Substring(0, 10); // Giving name of a file 10 symbols 

                // Concatenation of path, file name and type 
                string sourcePath = path + nameFile + fileType;


                // Збережемо співідношення сторін
                int newWidth = image.Width;
                int newHeight = image.Height;
                int imageHorizontalResolution = Convert.ToInt32(image.HorizontalResolution);

                int x = (newWidth - newHeight) / 2;
                if (x < 0)
                    x = 0;
                int y = (newHeight - newWidth) / 2;
                if (y < 0)
                    y = 0;

                // Отримаєм потік і обріжимо за заданими кординатами
                int imageWidth = newWidth;
                int imageHigth = newHeight;
                int newX = 0;
                int newY = 0;

                Image resizeImage = new Bitmap(w, h);
                Graphics graphics = Graphics.FromImage(resizeImage);


                //Resize picture according to size
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                graphics.DrawImage(image, new Rectangle(newX, newY, w, w), new Rectangle(x, y, newWidth - x * 2, newHeight - y * 2), GraphicsUnit.Pixel);


                var bmpStream = new MemoryStream();
                resizeImage.Save(bmpStream, image.RawFormat);
                bmpStream.Position = 0;
                var mimeType = SupportMimeTypeOfStream(image.RawFormat.Guid);

                var result = await UploadFileToAzureAsync(bmpStream, sourcePath, mimeType);
                uploadImageResult.Error = result.Error;
                uploadImageResult.PathFile = result.PathFile;
                image.Dispose();
                return uploadImageResult;
            }
            catch (Exception e)
            {
                uploadImageResult.Error += e.Message.ToString();
                return uploadImageResult;
            }
        }
        /*-----------------------------------------------------------------*/
        public async Task<UploadBaseResult> UploadFileImage(HttpPostedFile inputFile, string path = null, int resizeWidth = 0, int minWidth = 10, int minHeigth = 10)
        {
            try
            {
                string nameFile;

                // If path null save to temporary directory 
                if (path == null)
                    path = UploadImageProperties.TemporaryFolder;

                if (inputFile == null)
                {
                    uploadImageResult.Error = ErrorDescriptions.FileMissing;

                    return uploadImageResult;
                }

                // Obtaining Content Type 
                string fileType = SupportMimeType(inputFile.ContentType);

                // Checking filr for errors 
                bool checkFIle = CheckFile(inputFile.InputStream, fileType, minWidth, minHeigth);

                if (!checkFIle)
                {
                    uploadImageResult.Error += error;

                    return uploadImageResult;
                }

                nameFile = System.Guid.NewGuid().ToString("N").Substring(0, 10); // Giving name of a file 10 symbols 

                // Concatenation of path, file name and type 
                string sourcePath = path + nameFile + fileType;

                // Calling method for file loading on Azure
                if (resizeWidth == 0)
                {
                    Image image = Bitmap.FromStream(inputFile.InputStream);
                    SetImageOrientation(ref image);
                    string mimeType = SupportMimeTypeOfStream(image.RawFormat.Guid);
                    var bmpStream = new MemoryStream();
                    image.Save(bmpStream, ImageFormat.Jpeg);
                    bmpStream.Position = 0;

                    // Завантаження потоку на Azure BLOB
                    var result = await UploadFileToAzureAsync(bmpStream, sourcePath, mimeType);
                    uploadImageResult.Error = result.Error;
                    uploadImageResult.PathFile = result.PathFile;
                }
                else
                {
                    await ResizeImage(inputFile.InputStream, sourcePath, resizeWidth, nameFile + fileType);
                }

                return uploadImageResult;
            }
            catch (Exception e)
            {
                uploadImageResult.Error += e.Message.ToString();

                return uploadImageResult;
            }
        }

        public async Task<UploadBaseResult> ResizeImage(Stream stream, string sourcePath, int w, string nameFile)
        {
            string nameResizeImage = nameFile;
            string pathResizeImage = sourcePath + nameResizeImage;

            try
            {
                Image image = Bitmap.FromStream(stream);
                SetImageOrientation(ref image);

                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float ratio = (float)w / (float)originalWidth;
                int newWidth = 0;
                int newHeight = 0;
                int newX = 0;
                int newY = 0;

                if (w > originalWidth)
                {
                    newWidth = originalWidth;
                    newHeight = originalHeight;
                }
                else
                {
                    newWidth = (int)(originalWidth * ratio);
                    newHeight = (int)(originalHeight * ratio);
                }

                Bitmap resizeImage = new Bitmap(newWidth, newHeight);
                Graphics graphics = Graphics.FromImage(resizeImage);

                //Resize picture according to size
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                graphics.DrawImage(image, newX, newY, newWidth, newHeight);

                var bmpStream = new MemoryStream();
                resizeImage.Save(bmpStream, ImageFormat.Jpeg);
                bmpStream.Position = 0;
                string mimeType = SupportMimeTypeOfStream(image.RawFormat.Guid);
                var result = await UploadFileToAzureAsync(bmpStream, pathResizeImage, mimeType);

                uploadImageResult.PathFile = result.PathFile;

                image.Dispose();
                resizeImage.Dispose();
                graphics.Dispose();

                return uploadImageResult;
            }
            catch (Exception e)
            {
                return uploadImageResult;
            }

        }

        public async Task<UploadBaseResult> CropImage(CropImage model, string sourcePath = null, int w = 0)
        {
            try
            {
                float scale = float.Parse(model.Scale, CultureInfo.InvariantCulture);

                // Якщо шлях не задано збережемо в тимчасову папку
                if (sourcePath == null)
                    sourcePath = "/" + UploadImageProperties.TemporaryThumbnailFolder;

                // Ім'я зображення
                string fileName = model.PathPhoto.Substring(model.PathPhoto.LastIndexOf("/") + 1);

                // Повний шлях зображення
                uploadImageResult.PathFile = "/" + containerName + "/" + sourcePath + fileName;

                // Шлях для завантаження на Azure BLOB
                sourcePath = sourcePath + fileName;

                // Вхідний шлях для файла, обріжимо "/images/"
                string inputPath = model.PathPhoto.Replace("/images/", "");

                if (scale > 0)
                {
                    model.Width = (int)(model.Width / scale);
                    model.Height = (int)(model.Height / scale);
                    model.x = (int)(model.x / scale);
                    model.y = (int)(model.y / scale);

                    // not use
                    //logger.Info("test:  " + "Width " + model.Width + "Height " + model.Height + "x " + model.x + "y " + model.y + "scale " + model.Scale);
                }

                // Отримаєм потік і обріжимо за заданими кординатами
                Image image = Bitmap.FromStream(await DownloadFileToStreamAsync(inputPath));
                SetImageOrientation(ref image);

                var bmp = new Bitmap(model.Width, model.Height, image.PixelFormat);
                var gra = Graphics.FromImage(bmp);
                gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                gra.DrawImage(image, new Rectangle(0, 0, model.Width, model.Height), new Rectangle(model.x, model.y, model.Width, model.Height), GraphicsUnit.Pixel);

                // Створимо новий потік
                var bmpStream = new MemoryStream();

                // Якщо ширина обрізаного зображення більше за змінну "w" то зменшимо розміри зображення до ширини "w"
                // якщо менше то збережемо зображення в потік
                if (model.Width > w && w != 0)
                    bmpStream = ResizeImage(bmp, uploadImageResult.PathFile, fileName, w);
                else
                    bmp.Save(bmpStream, image.RawFormat);

                // Обнулимо позицію потоку
                bmpStream.Position = 0;

                // Отримаємо тип зображення що б потім використати при завантаженні на Azure BLOB
                string mimeType = SupportMimeTypeOfStream(image.RawFormat.Guid);

                // Завантаження потоку на Azure BLOB
                await UploadFileToAzureAsync(bmpStream, sourcePath, mimeType);

                image.Dispose();
                bmp.Dispose();
                gra.Dispose();

                return uploadImageResult;
            }
            catch (Exception e)
            {
                uploadImageResult.Error = e.Message;

                return uploadImageResult;
            }
        }

        public async Task<UploadBaseResult> CropAndUpload(CropImage model, HttpPostedFileBase sourceImage, string sourcePath = null, int w = 0)
        {
            try
            {
                float scale = float.Parse(model.Scale, CultureInfo.InvariantCulture);

                // Якщо шлях не задано збережемо в тимчасову папку
                if (sourcePath == null)
                    sourcePath = "/" + UploadImageProperties.TemporaryThumbnailFolder;

                // Ім'я зображення
                string fileName = model.PathPhoto.Substring(model.PathPhoto.LastIndexOf("/") + 1);

                // Повний шлях зображення
                uploadImageResult.PathFile = "/" + containerName + "/" + sourcePath + fileName;

                // Шлях для завантаження на Azure BLOB
                sourcePath = sourcePath + fileName;

                // Вхідний шлях для файла, обріжимо "/images/"

                if (scale > 0)
                {
                    model.Width = (int)(model.Width / scale);
                    model.Height = (int)(model.Height / scale);
                    model.x = (int)(model.x / scale);
                    model.y = (int)(model.y / scale);

                    // not use
                    //logger.Info("test:  " + "Width " + model.Width + "Height " + model.Height + "x " + model.x + "y " + model.y + "scale " + model.Scale);
                }

                // Отримаєм потік і обріжимо за заданими кординатами
                Image image = Bitmap.FromStream(sourceImage.InputStream);
                SetImageOrientation(ref image);

                var bmp = new Bitmap(model.Width, model.Height, image.PixelFormat);
                var gra = Graphics.FromImage(bmp);
                gra.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                gra.DrawImage(image, new Rectangle(0, 0, model.Width, model.Height), new Rectangle(model.x, model.y, model.Width, model.Height), GraphicsUnit.Pixel);

                // Створимо новий потік
                var bmpStream = new MemoryStream();

                // Якщо ширина обрізаного зображення більше за змінну "w" то зменшимо розміри зображення до ширини "w"
                // якщо менше то збережемо зображення в потік
                if (model.Width > w && w != 0)
                    bmpStream = ResizeImage(bmp, uploadImageResult.PathFile, fileName, w);
                else
                    bmp.Save(bmpStream, image.RawFormat);

                // Обнулимо позицію потоку
                bmpStream.Position = 0;

                // Отримаємо тип зображення що б потім використати при завантаженні на Azure BLOB
                string mimeType = SupportMimeTypeOfStream(image.RawFormat.Guid);

                // Завантаження потоку на Azure BLOB
                await UploadFileToAzureAsync(bmpStream, sourcePath, mimeType);

                image.Dispose();
                bmp.Dispose();
                gra.Dispose();

                return uploadImageResult;
            }
            catch (Exception e)
            {
                uploadImageResult.Error = e.Message;

                return uploadImageResult;
            }
        }


        /// <summary>
        /// Провірки зображення на валідність
        /// </summary>
        /// <param name="url"></param>
        /// <param name="minWidth"></param>
        /// <param name="minHeight"></param>
        /// <returns></returns>
        public bool CheckFileForUrl(string url, int minWidth, int minHeight)
        {
            // Максимальний розмір файла в кб
            maxLengthFile = 10240;
            try
            {
                // Налаштуємо запит
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "GET";
                webRequest.KeepAlive = false;
                webRequest.PreAuthenticate = false;
                webRequest.Timeout = 5000;
                // Збережемо зображення в потік
                var response = webRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                // Свторимо зображення з потоку
                Image image = Bitmap.FromStream(stream);
                // Отримаємо тип зображення
                var contentType = SupportMimeTypeOfStream(image.RawFormat.Guid);
                // Отримаємо розширення зображення
                var mimeType = SupportMimeType(contentType);
                // Створимо новий потік
                var imageStream = new MemoryStream();
                // Збережемо зображення в потік
                image.Save(imageStream, image.RawFormat);
                // Обнулимо позицію потоку
                imageStream.Position = 0;
                // Перевірка файлу на помилки
                bool checkFIle = CheckFile(imageStream, mimeType, minWidth, minHeight);
                if (!checkFIle)
                    return false;

                image.Dispose();
                imageStream.Dispose();
                stream.Dispose();
                response.Dispose();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }


        private MemoryStream ResizeImage(Image image, string sourcePath, string nameImage, int w)
        {
            try
            {
                // Находим "/" з кінця і обрізаємо все що до нього включно, отримаємо ім'я файлу
                string nameResizeImage = nameImage;
                string pathResizeImage = sourcePath + nameResizeImage;

                // Збережемо співідношення сторін
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float ratio = (float)w / (float)originalWidth;
                int newWidth = (int)(originalWidth * ratio);
                int newHeight = (int)(originalHeight * ratio);

                Image myThumbnail = image.GetThumbnailImage(newWidth, newHeight, new Image.GetThumbnailImageAbort(() => { return false; }), IntPtr.Zero);

                var myThumbnailStream = new MemoryStream();

                //TODO розібратися з 2 параметром ImageFormat.Jpeg
                myThumbnail.Save(myThumbnailStream, ImageFormat.Jpeg);
                image.Dispose();

                return myThumbnailStream;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private bool CheckFile(Stream stream, string mimeType, int minWidth = 0, int minHeigth = 0)
        {
            try
            {
                if (stream == null)
                {
                    error = "Stream null";
                    return false;
                }

                if (stream.Length > (maxLengthFile = maxLengthFile * 1024))
                {
                    error = $"Max size: {maxLengthFile}";
                    return false;
                }

                if (mimeType == null)
                {
                    error = "Unknown file";
                    return false;
                }

                // Якщо співдношення довжини більше за ширину в 2 рази то помилка
                Image image = Bitmap.FromStream(stream);
                //stream.Position = 0;

                if (image.Width < minWidth || image.Height < minHeigth)
                {
                    //error = MyResources.Resources.up_er_ImgSize + MyResources.Resources.up_er_ImgSize2 + " " + image.Width + " i " + MyResources.Resources.up_er_ImgSize3 + " " + image.Height + "." + " " + minWidth + " " + MyResources.Resources.up_er_ImgSizeHeigth + " "+minHeigth + ".";
                    //error = MyResources.Resources.up_er_ImgSize + " " + MyResources.Resources.up_er_ImgSize2 + " " +
                    //    image.Width + " " + MyResources.Resources.up_er_ImgSizeHeigth + " " + image.Height +
                    //    ". " + MyResources.Resources.up_er_ImgSize3 + " " + minWidth + " " +
                    //    MyResources.Resources.up_er_ImgSizeHeigth + " " + minHeigth + ".";
                    return false;
                }

                if (image.Width * 4 < image.Height || image.Width < minWidth)
                {
                    //error = MyResources.Resources.up_er_ImgRatio + " \n" + MyResources.Resources.up_er_ImgRatio2
                    //    + image.Width + MyResources.Resources.up_er_ImgRatio3 + image.Height + "\n" + MyResources.Resources.up_er_ImgRatio4 + "\n" + MyResources.Resources.up_er_ImgRatio5;
                    image.Dispose();
                    return false;
                }

                image.Dispose();
                return true;
            }
            catch (Exception e)
            {
                error = ErrorDescriptions.BadType;

                return false;
            }
        }

        private static string SupportMimeType(string mimeType)
        {
            switch (mimeType)
            {
                case "image/jpg":
                    return ".jpg";
                case "image/jpeg":
                    return ".jpeg";
                case "image/png":
                    return ".png";
                case "image/gif":
                    return ".gif";
            }
            return null;
        }

        private static string SupportMimeTypeOfStream(Guid id)
        {
            string mimeType = null;

            if (id == ImageFormat.Png.Guid)
                mimeType = "image/png";
            else if (id == ImageFormat.Gif.Guid)
                mimeType = "image/gif";
            else if (id == ImageFormat.Jpeg.Guid)
                mimeType = "image/jpeg";

            return mimeType;
        }

        private static void SetImageOrientation(ref Image image)
        {
            if (Array.IndexOf(image.PropertyIdList, 274) > -1)
            {
                var orientation = (int)image.GetPropertyItem(274).Value[0];
                switch (orientation)
                {
                    case 1:
                        // No rotation required.
                        break;
                    case 2:
                        image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3:
                        image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4:
                        image.RotateFlip(RotateFlipType.Rotate180FlipX);
                        break;
                    case 5:
                        image.RotateFlip(RotateFlipType.Rotate90FlipX);
                        break;
                    case 6:
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7:
                        image.RotateFlip(RotateFlipType.Rotate270FlipX);
                        break;
                    case 8:
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                }

                // This EXIF data is now invalid and should be removed.
                image.RemovePropertyItem(274);
            }

        }
    }
}
