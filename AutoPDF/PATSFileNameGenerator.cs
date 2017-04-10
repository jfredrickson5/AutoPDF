using System.IO;
using System.Linq;

namespace AutoPDF
{
    class PATSFileNameGenerator : IFileNameGenerator
    {
        public string GenerateFileName(Form currentForm, int currentIndex, int numRecords, string templateFile, string inputFile)
        {
            var lastName = currentForm.Fields["Last"];
            var firstName = currentForm.Fields["First"];
            var middleName = currentForm.Fields["MI"];
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
