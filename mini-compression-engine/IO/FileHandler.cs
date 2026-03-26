using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_compression_engine.IO;

public static class FileHandler
{
    public static byte[] Read(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"File not found: {path}");

        return File.ReadAllBytes(path);
    }

    public static void Write(string path, byte[] data)
    {
        File.WriteAllBytes(path, data);
    }
}
