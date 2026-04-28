namespace mini_compression_engine.IO;

public class BitWriter
{
    private byte CurrentByte = 0;
    private List<byte> ByteData { get; set; } = new();
    int CurrentBitCount = 0;

    public void Write(bool bitToUse)
    {
        byte bit = Convert.ToByte(bitToUse);
        CurrentByte = (byte)(CurrentByte << 1);
        CurrentByte = (byte)(CurrentByte | bit);
        CurrentBitCount++;

        if (CurrentBitCount == 8)
        {
            ByteData.Add(CurrentByte);
            CurrentBitCount = 0;
            CurrentByte = 0;
        }
    }

    public void Write(List<bool> bits)
    {
        foreach (bool bit in bits)
        {
            Write(bit);
        }
    }

    public void Flush()
    {
        if (CurrentBitCount == 0)
        {
            return;
        }

        CurrentByte = (byte)(CurrentByte << (8 - CurrentBitCount));
        ByteData.Add(CurrentByte);

        CurrentBitCount = 0;
        CurrentByte = 0;
    }

    public byte[] ToArray()
    {
        Flush();
        return ByteData.ToArray();
    }
}
