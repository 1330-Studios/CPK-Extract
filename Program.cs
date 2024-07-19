using System.Collections.Concurrent;
using CriFsV2Lib.Definitions.Utilities;

namespace CpkExtract;

internal class Program {
    internal static void Main(string[] args) {
        if (args.Length == 0 || args.Contains("--help") || args.Contains("-h")) {
            Console.WriteLine("Usage: CpkExtract.exe <file-path> [output-directory]");
            Console.WriteLine("  <file-path>          The path to the directory or file to search for .cpk files.");
            Console.WriteLine("  [output-directory]   Optional. The directory where extracted files will be stored. Defaults to the same directory as the .cpk file.");
            return;
        }

        var path = args[0];
        var outputDir = args.Length > 1 ? args[1] : null;

        // If path is either directory or file path
        if (!(Directory.Exists(path) || File.Exists(path))) {
            Console.WriteLine($"Error: The specified path '{path}' does not exist.");
            Console.WriteLine("Usage: CpkExtract.exe <file-path> [output-directory]");
            Console.WriteLine("  <file-path>          The path to the directory or file to search for .cpk files.");
            Console.WriteLine("  [output-directory]   Optional. The directory where extracted files will be stored. Defaults to the same directory as the .cpk file.");
            return;
        }

        var extractor = new ExtractorContext(outputDir);

        var directoryMode = Directory.Exists(path);

        if (directoryMode) {
            var files = Directory.EnumerateFiles(Path.GetDirectoryName(path)!, "*.cpk", SearchOption.AllDirectories).Where(a => !File.GetAttributes(a).HasFlag(FileAttributes.Directory));
            var partitioner = Partitioner.Create(files, EnumerablePartitionerOptions.NoBuffering);

            Parallel.ForEach(partitioner, extractor.Extract);
        } else {
            extractor.Extract(path);
        }
        
        extractor.LogErrors();

        ArrayRental.Reset();
    }
}