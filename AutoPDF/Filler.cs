using CsvHelper;
using System;
using System.IO;
using System.Linq;

namespace AutoPDF
{
    class Filler
    {
        public string TemplateFile { get; set; }
        public string InputFile { get; set; }
        public string InputDelimiter { get; set; }
        public string OutputDirectory { get; set; }
        public int NumRecords { get; private set; }
        public string[] FieldNames { get; private set; }

        public Filler(string templateFile, string inputFile, string outputDirectory, string inputDelimiter = ",")
        {
            TemplateFile = templateFile;
            InputFile = inputFile;
            OutputDirectory = outputDirectory;
            InputDelimiter = inputDelimiter;

            using (var textReader = File.OpenText(InputFile))
            {
                var csv = new CsvReader(textReader);
                csv.Configuration.Delimiter = InputDelimiter;
                csv.ReadHeader();
                FieldNames = csv.FieldHeaders;
            }

            NumRecords = File.ReadLines(InputFile).Count() - 1;
        }
        
        public void Fill()
        {
            using (var textReader = File.OpenText(InputFile))
            {
                var csv = new CsvReader(textReader);
                csv.Configuration.Delimiter = InputDelimiter;
                var index = 0;
                while (csv.Read())
                {
                    index++;
                    var form = new Form(TemplateFile);
                    foreach (var fieldName in FieldNames)
                    {
                        var fieldValue = csv.GetField(fieldName);
                        form.Fields.Add(fieldName, fieldValue);
                    }
                    using (var file = new FileStream(GenerateFilePath(OutputDirectory, NumRecords, index), FileMode.Create))
                    {
                        form.GetStream().WriteTo(file);
                    }
                }
            }
        }

        private string GenerateFilePath(string directory, int numRecords, int index)
        {
            var format = new String('0', numRecords.ToString().Length);
            var filename = index.ToString(format) + ".pdf";
            return Path.Combine(directory, filename);
        }
    }
}
