using System;
using System.Collections.Generic;
using AutoPDF.Plugin;

namespace AutoPDF
{
    class DefaultFileNameGenerator : IFileNameGenerator
    {
        public string GenerateFileName(IDictionary<string, object> formFields, int currentIndex, int numRecords, string templateFile, string inputFile)
        {
            var format = new String('0', numRecords.ToString().Length);
            var fileName = currentIndex.ToString(format) + ".pdf";
            return fileName;
        }
    }
}
