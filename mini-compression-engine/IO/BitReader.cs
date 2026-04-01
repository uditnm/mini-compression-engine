namespace mini_compression_engine.IO;

public class BitReader
{
    public BitReader(byte[] byteData, int bitCount)
    {
        ByteData = byteData;
        BitCount = bitCount;
    }

    private byte[] ByteData { get; set; }
    private int BitCount { get; set; }
    private int CurrentBitIndex = 8;
    private int CurrentByteIndex = 0;
    private int BitsRead = 0;

    public bool Read()
    {
        if (CurrentBitIndex == 0)
        {
            CurrentBitIndex = 8;
            CurrentByteIndex++;
        }

        if (CurrentByteIndex >= ByteData.Length)
        {
            throw new InvalidOperationException("No more data to read.");
        }
        var data = ByteData[CurrentByteIndex];

        if (BitsRead < BitCount)
        {
            var bit = (data >> (CurrentBitIndex - 1)) & 1;
            CurrentBitIndex--;
            BitsRead++;
            return Convert.ToBoolean(bit);
        }

        throw new InvalidOperationException("No more data to read.");
    }
}
