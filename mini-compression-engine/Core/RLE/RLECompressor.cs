using mini_compression_engine.Core.Interfaces;

namespace mini_compression_engine.Core.RLE;

internal class RLECompressor : ICompressor
{
    public byte[] Compress(byte[] input)
    {
        var encoded = new List<byte>();
        byte currentByte = input[0];
        int count = 1;

        for(int i = 1; i < input.Length; i++)
        {
            if (input[i] == currentByte && count < 255)
            {
                count++;
            }
            else
            {
                encoded.Add(currentByte);
                encoded.Add((byte)count);
                currentByte = input[i];
                count = 1;
            }
        }

        // Add the last byte and count
        encoded.Add(currentByte);
        encoded.Add((byte)count);

        return encoded.ToArray();
    }

    public byte[] Decompress(byte[] input)
    {
        var decoded = new List<byte>();
        for(int i = 0; i < input.Length; i += 2)
        {
            byte value = input[i];
            byte count = input[i + 1];
            for(int j = 0; j < count; j++)
            {
                decoded.Add(value);
            }
        }

        return decoded.ToArray();
    }
}
