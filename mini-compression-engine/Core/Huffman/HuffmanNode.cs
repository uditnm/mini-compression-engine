namespace mini_compression_engine.Core.Huffman;

public class HuffmanNode
{
    public HuffmanNode(HuffmanNode left = null, HuffmanNode right = null)
    {
        Left = left;
        Right = right;
    }
    public byte? Value {  get; set; }
    public int Frequency {  get; set; }
    public HuffmanNode Left { get; set; }
    public HuffmanNode Right { get; set; }
}