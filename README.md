# Simple QR combiner / reader
Learning ImageSharp and ZXing.

Simple program that takes QR code parts and variously combines them
trying to find combination that produces valid QR code (and decode it).


Each folder in `data` represents cell in 3x3 grid (1-2-3 is first row, 4-5-6 second etc)
Each file name in cell folder represents part number that can be put on this cell. Parts count is also 9 and
all parts must be placed on all cells (one part cannot be placed on more than one cell).

If file name contains more than one digit then first digit is the part number and other is just number
used to separate various transformation of this part. In this case there are 4 rotation for 3 parts in
2, 4 and 5 cells.

Program steps:
* Load images
* Generate all possible parts combinations
* For each combination generate image and try to read QR from it (using Parallel.ForEach())

Generated images also stored in the `out` folder.