using System;
using System.IO;
using System.IO.Compression;

namespace AutoPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = Options.Instance;
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
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
                Logger.Log(LogLevel.Info, "Using temporary directory: " + outputDirectory);
                Directory.CreateDirectory(outputDirectory);
            }

            var pluginManager = PluginManager.Instance;

            Filler filler;
            if (String.IsNullOrEmpty(options.Delimiter))
            {
                Logger.Log(LogLevel.Info, "No delimiter provided; guessing based on the input file header");
                filler = new Filler(options.TemplateFile, options.InputFile, outputDirectory);
            }
            else
            {
                Logger.Log(LogLevel.Info, "Using delimiter: " + options.Delimiter);
                filler = new Filler(options.TemplateFile, options.InputFile, outputDirectory, options.Delimiter);
            }

            if (pluginManager.InputParserPlugins.Count > 0)
            {
                Logger.Log(LogLevel.Info, "Using input parser plugin: " + pluginManager.InputParserPlugins[0].GetType().Name);
                filler.InputParser = pluginManager.InputParserPlugins[0];
            }
            if (pluginManager.FileNameGeneratorPlugins.Count > 0)
            {
                Logger.Log(LogLevel.Info, "Using file name generator plugin: " + pluginManager.FileNameGeneratorPlugins[0].GetType().Name);
                filler.FileNameGenerator = pluginManager.FileNameGeneratorPlugins[0];
            }

            filler.Fill();
            
            if (options.UseZipFile)
            {
                if (File.Exists(options.Destination))
                {
                    Logger.Log(LogLevel.Info, "Removing existing zip file: " + options.Destination);
                    File.Delete(options.Destination);
                }
                Logger.Log(LogLevel.Info, "Creating zip file: " + options.Destination);
                ZipFile.CreateFromDirectory(outputDirectory, options.Destination, CompressionLevel.Optimal, false);
                Logger.Log(LogLevel.Info, "Removing temporary directory: " + outputDirectory);
                Directory.Delete(outputDirectory, true);
            }
        }
    }
}
