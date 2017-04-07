using System.IO;
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
            var outputDirectory = destination;

            if (useZipFile)
            {
                outputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                Directory.CreateDirectory(outputDirectory);
            }

            var filler = new Filler(templateFile, inputFile, outputDirectory);
            filler.Fill();
            
            if (useZipFile)
            {
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }
                ZipFile.CreateFromDirectory(outputDirectory, destination, CompressionLevel.Optimal, false);
                Directory.Delete(outputDirectory, true);
            }
        }
    }
}
