using CommandLine;
using CommandLine.Text;
using System;
using System.IO;

namespace AutoPDF
{
    class Options
    {
        [HelpOption]
        public string Usage()
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = System.Reflection.AssemblyName.GetAssemblyName(assembly.Location).Version.ToString();

            var helpText = new HelpText
            {
                Heading = new HeadingInfo("AutoPDF", version),
                AddDashesToOption = true
            };
            helpText.AddPreOptionsLine("Usage: " + appName + " TEMPLATEFILE INPUTFILE DESTINATION [options]");
            helpText.AddOptions(this);

            return helpText;
        }

        [Option('d', "delimiter", Required = false, HelpText = "Delimiter used in the input file (default: autodetect)")]
        public string Delimiter { get; set; }
        
        [ValueOption(0)]
        public string TemplateFile { get; set; }

        [ValueOption(1)]
        public string InputFile { get; set; }

        [ValueOption(2)]
        public string Destination { get; set; }

        public bool UseZipFile
        {
            get { return Destination.EndsWith(".zip"); }
        }

        public OptionsValidationResult Validate()
        {
            var result = new OptionsValidationResult(true);

            if (!File.Exists(TemplateFile))
            {
                result.Valid = false;
                result.Message = "Template does not exist: " + TemplateFile;
            }
            if (!File.Exists(InputFile))
            {
                result.Valid = false;
                result.Message = "Input file does not exist: " + InputFile;
            }
            if (!UseZipFile && !Directory.Exists(Destination))
            {
                result.Valid = false;
                result.Message = "Destination directory does not exist: " + Destination;
            }
            if (UseZipFile && !Directory.Exists(Path.GetDirectoryName(Destination)))
            {
                result.Valid = false;
                result.Message = "Destination directory does not exist: " + Path.GetDirectoryName(Destination);
            }

            return result;
        }
    }
}
