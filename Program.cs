using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using ZXing;

namespace qr;

internal static class Program
{
    private static void Main()
    {
        var quads = Enumerable
            .Range(1, 9)
            .Select(n => Quad.LoadFromFolder($"data/{n}"))
            .ToList();

        var listsOfParts = new List<List<Part>>();
        GenerateListsOfParts(quads, 0, new HashSet<int>(), new List<Part>(), listsOfParts);

        Parallel.ForEach(
            Enumerable.Range(0, listsOfParts.Count).Select(n => (n, listsOfParts[n])),
            pair => ConstructAndDecodeQrCode($"out/{pair.Item1:D3}.png", pair.Item2));
    }

    private static void ConstructAndDecodeQrCode(string path, List<Part> listOfParts)
    {
        var qrCodeImage = new Image<Rgb24>(300, 300, new Rgb24(255, 255, 255));
        for (var y = 0; y < 3; y++)
        {
            for (var x = 0; x < 3; x++)
            {
                var part = listOfParts[x + y * 3];
                qrCodeImage.Mutate(context => context.DrawImage(part.Image, new Point(x * 100, y * 100), 1.0f));
            }
        }
        
        // comment to skip saving QR code to /out and speed up function
        qrCodeImage.Save(path);
        
        var data = new byte[qrCodeImage.Width * qrCodeImage.Height * 3];
        qrCodeImage.CopyPixelDataTo(new Span<byte>(data));
        
        var result = new BarcodeReaderGeneric().Decode(data, qrCodeImage.Width, qrCodeImage.Height, RGBLuminanceSource.BitmapFormat.RGB24);
        if(result != null)
            Console.WriteLine(result);
    }

    private static void GenerateListsOfParts(IReadOnlyList<Quad> quads, int id, ISet<int> takenNumbers, IList<Part> parts, ICollection<List<Part>> listsOfParts)
    {
        if (id == quads.Count)
        {
            listsOfParts.Add(new List<Part>(parts));
            return;
        }
        
        var quad = quads[id];
        foreach (var (number, quadParts) in quad._parts)
        {
            if(takenNumbers.Contains(number))
                continue;
            
            takenNumbers.Add(number);
            foreach (var part in quadParts)
            {
                parts.Add(part);
                GenerateListsOfParts(quads, id + 1, takenNumbers, parts, listsOfParts);
                parts.RemoveAt(parts.Count - 1);
            }
            takenNumbers.Remove(number);
        }
    }
}