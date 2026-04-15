using mini_compression_engine.Core.Interfaces;
using mini_compression_engine.IO;

namespace mini_compression_engine.Core.Huffman;

public class HuffmanCompressor : ICompressor
{
    public byte[] Compress(byte[] input)
    {
        if (input.Length == 0)
        {
            return Array.Empty<byte>();
        }

        var frequencyTable = GetFrequency(input);
        var minHeap = new PriorityQueue<HuffmanNode, int>();
        foreach (var frequency in frequencyTable)
        {
            minHeap.Enqueue(new HuffmanNode { Value = frequency.Key, Frequency = frequency.Value }, frequency.Value);
        }

        //build huffman tree
        while(minHeap.Count > 1)
        {
            var smallest = minHeap.Dequeue();
            var secondSmallest = minHeap.Dequeue();

            var newNode = new HuffmanNode
            {
                Frequency = smallest.Frequency + secondSmallest.Frequency,
                Left = smallest,
                Right = secondSmallest
            };

            minHeap.Enqueue(newNode, newNode.Frequency);
        }

        var headNode = minHeap.Dequeue();

        //generate codes for each byte by traversing tree
        var codes = GenerateCodes(headNode);

        //encode
        var writer = new BitWriter();
        int bitCount = 0;
        foreach(var data in input)
        {
            var code = codes[data];
            writer.Write(code);
            bitCount += code.Count;
        }

        var metaData = new List<byte>();

        // Serialize the frequency table as a sequence of (byte, int) pairs.
        // First, store the count of unique bytes.
        metaData.AddRange(BitConverter.GetBytes(frequencyTable.Count));
        foreach (var kvp in frequencyTable)
        {
            metaData.Add(kvp.Key);
            metaData.AddRange(BitConverter.GetBytes(kvp.Value));
        }

        //store bitCount
        metaData.AddRange(BitConverter.GetBytes(bitCount));

        var compressedBytes = writer.ToArray();
        var finalOutput = new List<byte>(metaData);
        finalOutput.AddRange(compressedBytes);

        return finalOutput.ToArray();
    }

    private Dictionary<byte, List<bool>> GenerateCodes(HuffmanNode headNode)
    {
        var codes = new Dictionary<byte, List<bool>>();
        if(headNode.Left == null && headNode.Right == null)
        {
            codes[headNode.Value.Value] = new List<bool> { false };
            return codes;
        }

        Traverse(headNode, codes, new List<bool>());

        return codes;
    }

    private void Traverse(HuffmanNode node, Dictionary<byte, List<bool>> allCodes, List<bool> code)
    {
        if (node == null)
            return;

        if(node.Left == null && node.Right == null)
        {
            allCodes[node.Value.Value] = new List<bool>(code);
            return;
        }

        code.Add(false);
        Traverse(node.Left, allCodes, code);
        code.RemoveAt(code.Count - 1);

        code.Add(true);
        Traverse(node.Right, allCodes, code);
        code.RemoveAt(code.Count - 1);
    }

    private Dictionary<byte, int> GetFrequency(byte[] input)
    {
        var frequency = new Dictionary<byte, int>();
        foreach(var item in input)
        {
            frequency[item] = frequency.GetValueOrDefault(item) + 1;
        }
        return frequency;
    }

    public byte[] Decompress(byte[] input)
    {
        throw new NotImplementedException();
    }
}
