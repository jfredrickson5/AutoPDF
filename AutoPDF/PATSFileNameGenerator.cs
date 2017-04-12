using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoPDF
{
    class PATSFileNameGenerator : IFileNameGenerator
    {
        public string GenerateFileName(IDictionary<string, object> formFields, int currentIndex, int numRecords, string templateFile, string inputFile)
        {
            // Incoming input files will have the format YEAR_ORG_TIMESTAMP.txt (ex: 2014_ENA_1491500013.txt)
            var lastName = formFields["Last"];
            var firstName = formFields["First"];
            var middleName = formFields["MI"].ToString().Replace(".", "");
            var timestamp = GetTimestamp(inputFile);
            var fileName = lastName + "_" + firstName + "_" + middleName + "_" + timestamp + ".pdf";
            return fileName;
        }

        private string GetTimestamp(string inputFileName)
        {
            var baseName = Path.GetFileNameWithoutExtension(inputFileName);
            return baseName.Split('_').Last();
        }
    }
}
