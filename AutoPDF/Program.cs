using System;
using System.IO;
using System.IO.Compression;

namespace AutoPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(options.Usage());
                Environment.Exit(1);
            }
            var customValidation = options.Validate();
            if (!customValidation.Valid)
            {
                Console.WriteLine(options.Usage());
                Console.WriteLine("Error: " + customValidation.Message);
                Environment.Exit(1);
            }

            var outputDirectory = options.Destination;

            if (options.UseZipFile)
            {
                outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(outputDirectory);
            }

            var filler = String.IsNullOrEmpty(options.Delimiter) ?
                new Filler(options.TemplateFile, options.InputFile, outputDirectory) :
                new Filler(options.TemplateFile, options.InputFile, outputDirectory, options.Delimiter);
            filler.Fill();
            
            if (options.UseZipFile)
            {
                if (File.Exists(options.Destination))
                {
                    File.Delete(options.Destination);
                }
                ZipFile.CreateFromDirectory(outputDirectory, options.Destination, CompressionLevel.Optimal, false);
                Directory.Delete(outputDirectory, true);
            }
        }
    }
}
