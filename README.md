# mini-compression-engine



A lightweight project exploring lossless data compression techniques on real files.



### Implemented Algorithms



#### Run-Length Encoding (RLE)



Run-Length Encoding (RLE) is a lossless compression algorithm that reduces data size by replacing consecutive repeated values with a single value and a count.



##### Example:

AAAAA → A × 5



Binary representation:

\[value]\[count]



##### Results

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





### Usage



compress rle <input> <output>

decompress rle <input> <output>



Example for RLE:

dotnet run compress rle OnlyA.txt OnlyA\_compressed.rle

dotnet run decompress rle OnlyA\_compressed.rle OnlyA\_decompressed.txt





