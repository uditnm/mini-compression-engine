using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mini_compression_engine.IO;

public class BitWriter
{
    public byte CurrentByte = 0;
    public List<byte> ByteData { get; set; } = new();
    int BitCount = 0;
    public void Write(bool bitToUse)
    {
        byte bit = Convert.ToByte(bitToUse);
        CurrentByte = (byte)(CurrentByte << 1);
        CurrentByte = (byte)(CurrentByte | bit);
        BitCount++;

        if (BitCount == 8)
        {
            ByteData.Add(CurrentByte);
            BitCount = 0;
            CurrentByte = 0;
        }
    }

    public void Flush()
    {
        if (BitCount == 0)
        {
            return;
        }

        CurrentByte = (byte)(CurrentByte << (8 - BitCount));
        ByteData.Add(CurrentByte);

        BitCount = 0;
        CurrentByte = 0;
    }
}
