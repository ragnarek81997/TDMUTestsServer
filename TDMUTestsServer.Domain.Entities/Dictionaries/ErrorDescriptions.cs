using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDMUTestsServer.Domain.Entities.Dictionaries
{
    public static class ErrorDescriptions
    {
        public static string FileExists => Errors["FileExists"];
        public static string BadType => Errors["BadType"];
        public static string FileMissing => Errors["FileMissing"];
        public static string UserIsNull => Errors["UserIsNull"];
        public static string InvalidToken => Errors["InvalidToken"];
        public static string DescriptionMaxLength250 => Errors["DescriptionMaxLength250"];
        public static string NameMaxLength50 => Errors["NameMaxLength50"];
        public static string MaxFileSize => Errors["MaxFileSize"];
        public static string ExpNull => Errors["ExpNull"];
        public static string BadLink => Errors["BadLink"];

        private static readonly Dictionary<string, string> Errors = new Dictionary<string, string>()
        {
            {"FileExists", "File exists replacement is not possible." },
            {"BadType", "File type is bad, try other." },
            {"FileMissing", "File Missing." },
            {"UserIsNull", "User does not exist." },
            {"InvalidToken", "Token is invalid." },
            {"DescriptionMaxLength250", "The maximum length of description 250." },
            {"NameMaxLength50", "The maximum length of name 50." },
            {"GoalRequired", "Name and description is required." },
            {"GoalInvalidDate", "Invalid date. The date can't be earlier than the current one." },
            {"MaxFileSize", "Invalid file size. Max file size 20mb." },
            {"ExpNull","Experience null" },
            {"BadLink","Invalid url" }
        };

    }
}
