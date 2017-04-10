using System;
using System.IO;

namespace AutoPDF
{
    class DefaultFileNameGenerator : IFileNameGenerator
    {
        public string GenerateFileName(Form currentForm, int currentIndex, int numRecords, string templateFile, string inputFile)
        {
            var format = new String('0', numRecords.ToString().Length);
            var fileName = currentIndex.ToString(format) + ".pdf";
            return fileName;
        }
    }
}
