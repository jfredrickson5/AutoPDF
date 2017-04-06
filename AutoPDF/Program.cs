using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using System.IO.Compression;

namespace AutoPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var templateFile = args[0];
            var inputFile = args[1];
            var destination = args[2];
            
            var useZipFile = destination.EndsWith(".zip");
            
            var numRecords = File.ReadLines(inputFile).Count();

            using (var textReader = File.OpenText(inputFile))
            {
                var csv = new CsvReader(textReader);
                csv.Configuration.Delimiter = ":";
                csv.ReadHeader();
                var pdfDestination = destination;
                string tempDir = null;

                if (useZipFile)
                {
                    tempDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                    Directory.CreateDirectory(tempDir);
                    pdfDestination = tempDir;
                }

                var index = 0;
                while (csv.Read())
                {
                    index++;
                    var form = new Form(templateFile);
                    foreach (var fieldName in csv.FieldHeaders)
                    {
                        var fieldValue = csv.GetField(fieldName);
                        form.Fields.Add(fieldName, fieldValue);
                    }
                    using (var file = new FileStream(GenerateFilePath(pdfDestination, numRecords, index), FileMode.Create))
                    {
                        form.GetStream().WriteTo(file);
                    }
                }

                if (useZipFile)
                {
                    if (File.Exists(destination))
                    {
                        File.Delete(destination);
                    }
                    ZipFile.CreateFromDirectory(tempDir, destination, CompressionLevel.Optimal, false);
                }
            }
        }

        private static string GenerateFilePath(string destination, int numRecords, int index)
        {
            var format = new String('0', numRecords.ToString().Length);
            var filename = index.ToString(format) + ".pdf";
            return Path.Combine(destination, filename);
        }
    }
}
