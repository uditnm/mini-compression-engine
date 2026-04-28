# mini-compression-engine



A .NET-based compression engine implementing classic lossless algorithms
(RLE and Huffman) from scratch, including custom bit-level I/O,
binary file formats, and real-file benchmarks.


## Compression Results


| Dataset | RLE | Huffman |
|---------|-----|---------|
| Frankenstein | 196.63%  | 56.78% |
| 10MB of 'A' | 0.78%  | 12.50% |

## Implemented Algorithms



### Run-Length Encoding (RLE)



Run-Length Encoding (RLE) is a lossless compression algorithm that reduces data size by replacing consecutive repeated values with a single value and a count.



#### Example:
AAAAA → A × 5



Binary representation:
\[value]\[count]



#### Results

Real-world text (Frankenstein e-book)

Original:   448,888 bytes
Compressed: 882,666 bytes
Ratio:      196.63%



Increased size due to low repetition as most characters were encoded as \[value,1]



Best case (10MB of 'A')

Original:   10,485,760 bytes
Compressed:     82,242 bytes
Ratio:          0.78%



\~128× compression

The long runs were efficiently encoded



**Takeaway**: RLE works well on consecutive repetition, but fails on typical text.



### Huffman Coding



Huffman coding is a popular algorithm used for lossless data compression. The basic idea behind Huffman coding is to assign shorter codes to more frequent characters and longer codes to less frequent characters, thus reducing the overall size of the data.


## Huffman File Format

```text
| symbol count | frequency table | bit count | encoded data |
```


#### Example:

Suppose we build the Huffman Tree for “hello”:

Input frequencies:
l:2 h:1 e:1 o:1

Possible codes:
l -> 0
h -> 10
e -> 110
o -> 111



#### Results

Real-world text (Frankenstein e-book)

Original size:     448,888 bytes
Ratio:          56.78%

Encoded payload:   254,384 bytes
Metadata overhead:     508 bytes

Compressed size:   254,892 bytes
Reduction: 43.22%




Repeated symbol (10MB of 'A')

Original size:   10,485,760 bytes
Ratio:          12.50%

Encoded payload:   1,310,720 bytes
Metadata overhead:     13 bytes

Compressed size:   1,310,733 bytes
Reduction: 87.50%



## Metadata Overhead

- Frankenstein: 0.20%
- Repeated-symbol file: 0.001%


**Takeaway**: Huffman works well for natural language text.



## Internals

- Frequency-table based Huffman tree construction
- DFS code generation
- Custom bit packing / unpacking
- Metadata serialization for decompression



## Usage



```bash
compress rle [input-file] [output-file]
decompress rle [input-file] [output-file]
```



Example for RLE:

```bash
dotnet run compress rle OnlyA.txt OnlyA_compressed.rle
dotnet run decompress rle OnlyA_compressed.rle OnlyA_decompressed.txt
```


Example for Huffman:

```bash
dotnet run compress huffman OnlyA.txt OnlyA_compressed.hfmn
dotnet run decompress huffman OnlyA_compressed.hfmn OnlyA_decompressed.txt
```


## Key Insight

Compression performance is data dependent:

- RLE excels on long repeated runs.
- Huffman excels on natural language and skewed symbol distributions.
- No single algorithm is universally optimal.
