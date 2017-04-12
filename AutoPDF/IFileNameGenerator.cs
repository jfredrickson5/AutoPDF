using System.Collections.Generic;

namespace AutoPDF
{
    interface IFileNameGenerator
    {
        string GenerateFileName(IDictionary<string, object> formFields, int currentIndex, int numRecords, string templateFile, string inputFile);
    }
}
