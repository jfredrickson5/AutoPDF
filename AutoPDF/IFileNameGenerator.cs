namespace AutoPDF
{
    interface IFileNameGenerator
    {
        string GenerateFileName(Form currentForm, int currentIndex, int numRecords, string templateFile, string inputFile);
    }
}
