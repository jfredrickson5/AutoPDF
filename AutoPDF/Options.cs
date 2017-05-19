using CommandLine;
using CommandLine.Text;
using System;
using System.IO;

namespace AutoPDF
{
    class Options
    {
        private static Options _Instance;
        public static Options Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Options();
                }
                return _Instance;
            }
        }

        private Options() { }

        [HelpOption]
        public string Usage()
        {
            var appName = AppDomain.CurrentDomain.FriendlyName;
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = System.Reflection.AssemblyName.GetAssemblyName(assembly.Location).Version.ToString();
            var pluginList = PluginManager.Instance.PluginsString();

            var helpText = new HelpText
            {
                Heading = new HeadingInfo("AutoPDF", version),
                AddDashesToOption = true
            };
            helpText.AddPreOptionsLine("Active plugins: " + pluginList);
            helpText.AddPreOptionsLine("Usage: " + appName + " TEMPLATEFILE INPUTFILE DESTINATION [options]");
            helpText.AddOptions(this);

            return helpText;
        }

        [Option('d', "delimiter", Required = false, HelpText = "Delimiter used in the input file (default: autodetect)")]
        public string Delimiter { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Show additional information during processing")]
        public bool Verbose { get; set; }
        
        [ValueOption(0)]
        public string TemplateFile { get; set; }

        [ValueOption(1)]
        public string InputFile { get; set; }

        [ValueOption(2)]
        public string Destination { get; set; }

        public string DestinationDirectory
        {
            get
            {
                var dir = Path.GetDirectoryName(Destination);
                if (UseZipFile && String.IsNullOrEmpty(dir))
                {
                    dir = Directory.GetCurrentDirectory();
                }
                return dir;
            }
        }

        public bool UseZipFile
        {
            get {
                if (String.IsNullOrEmpty(Destination))
                {
                    return false;
                }
                return Destination.EndsWith(".zip");
            }
        }

        public OptionsValidationResult Validate()
        {
            if (String.IsNullOrEmpty(TemplateFile) || String.IsNullOrEmpty(InputFile) || String.IsNullOrEmpty(Destination))
            {
                return new OptionsValidationResult(false, "Missing required parameters");
            }
            if (!File.Exists(TemplateFile))
            {
                return new OptionsValidationResult(false, "Template does not exist: " + TemplateFile);
            }
            if (!File.Exists(InputFile))
            {
                return new OptionsValidationResult(false, "Input file does not exist: " + InputFile);
            }
            if (!UseZipFile && !Directory.Exists(Destination))
            {
                return new OptionsValidationResult(false, "Destination directory does not exist: " + Destination);
            }
            if (UseZipFile && !Directory.Exists(Path.GetDirectoryName(DestinationDirectory)))
            {
                return new OptionsValidationResult(false, "Destination directory does not exist: " + Path.GetDirectoryName(Destination));
            }
            return new OptionsValidationResult(true);
        }
    }
}
