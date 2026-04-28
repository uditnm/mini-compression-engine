using mini_compression_engine.Core.Huffman;
using mini_compression_engine.Core.Interfaces;
using mini_compression_engine.Core.RLE;
using mini_compression_engine.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length < 3)
        {
            PrintUsage();
            return;
        }

        string command = args[0].ToLower();     // compress / decompress
        string algorithm = args[1].ToLower();   // rle / huffman
        string inputPath = args[2];
        string outputPath = args.Length > 3 ? args[3] : GetDefaultOutput(command, inputPath);

        try
        {
            ICompressor compressor = GetCompressor(algorithm);

            if (command == "compress")
            {
                var input = FileHandler.Read(inputPath);
                var compressed = compressor.Compress(input);
                FileHandler.Write(outputPath, compressed);

                Console.WriteLine($"Compressed to {outputPath}");

                Console.WriteLine($"Original: {input.Length} bytes");
                Console.WriteLine($"Compressed: {compressed.Length} bytes");

                if (input.Length != 0)
                {
                    double ratio = (double)compressed.Length / input.Length * 100;
                    Console.WriteLine($"Ratio: {ratio:F2}%");
                }
            }
            else if (command == "decompress")
            {
                var input = FileHandler.Read(inputPath);
                var decompressed = compressor.Decompress(input);
                FileHandler.Write(outputPath, decompressed);

                Console.WriteLine($"Decompressed to {outputPath}");

                Console.WriteLine($"Original: {input.Length} bytes");
                Console.WriteLine($"Decompressed: {decompressed.Length} bytes");

                if(input.Length != 0)
                {
                    double ratio = (double)decompressed.Length / input.Length * 100;
                    Console.WriteLine($"Ratio: {ratio:F2}%");
                }
            }
            else
            {
                Console.WriteLine("Invalid command.");
                PrintUsage();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static ICompressor GetCompressor(string algorithm)
    {
        return algorithm switch
        {
            "rle" => new RLECompressor(),
            "huffman" => new HuffmanCompressor(),
            _ => throw new ArgumentException($"Unknown algorithm: {algorithm}")
        };
    }

    private static string GetDefaultOutput(string command, string inputPath)
    {
        return command == "compress"
            ? inputPath + ".cmp"
            : inputPath + ".out";
    }

    private static void PrintUsage()
    {
        Console.WriteLine("Usage:");
        Console.WriteLine("  compress <algorithm> <input> [output]");
        Console.WriteLine("  decompress <algorithm> <input> [output]");
        Console.WriteLine();
        Console.WriteLine("Supported algorithms: \n1. rle \n2. huffman");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine("  compress rle input.txt output.rle");
        Console.WriteLine("  decompress rle output.rle restored.txt");
    }
}