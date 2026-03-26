using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_compression_engine.Core.Interfaces;

public interface ICompressor
{
    public byte[] Compress(byte[] input);
    public byte[] Decompress(byte[] input);
}
