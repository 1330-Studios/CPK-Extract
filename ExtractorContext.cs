using System.Collections.Concurrent;
using System.IO.MemoryMappedFiles;
using CriFsV2Lib;

namespace CpkExtract;
public class ExtractorContext(string outputDirectory) {
    private readonly ConcurrentBag<(string, Exception)> m_errors = [];

    public void Extract(string file) {
        try {
            var outDir = outputDirectory ?? Path.Join(Path.GetDirectoryName(file), $"{Path.GetFileName(file)}_unpacked");

            using var mmf = MemoryMappedFile.CreateFromFile(file);
            using var accessor = mmf.CreateViewAccessor();
            var buffer = new byte[accessor.Capacity];
            accessor.ReadArray(0, buffer, 0, buffer.Length);

            using var memoryStream = new MemoryStream(buffer);
            using var reader = CriFsLib.Instance.CreateCpkReader(memoryStream, false);

            foreach (var cpkFile in reader.GetFiles()) {
                using var rented = reader.ExtractFile(cpkFile);
                var trimIndex = rented.Count - 1;
                while (trimIndex > 0 && rented.Span[trimIndex] == 0x00)
                    trimIndex--;

                Directory.CreateDirectory(Path.Join(outDir, cpkFile.Directory));
                File.WriteAllBytes(Path.Join(outDir, cpkFile.Directory, cpkFile.FileName), rented.Span[..(trimIndex + 1)].ToArray());
            }
        } catch (Exception e) {
            m_errors.Add((file, e));
        }
    }

    public void LogErrors() {
        foreach (var failedFile in m_errors) {
            Console.WriteLine($"Error: File {Path.GetFileName(failedFile.Item1)} failed - {failedFile.Item2.Message}");
        }
    }
}