namespace qr;

internal class Quad
{
    public readonly Dictionary<int, List<Part>> _parts = new();

    public static Quad LoadFromFolder(string path)
    {
        var quad = new Quad();
        
        var filePaths = Directory.GetFiles(path);
        foreach (var filePath in filePaths)
        {
            var number = Path.GetFileNameWithoutExtension(filePath)[0] - '0';
            if (!quad._parts.TryGetValue(number, out var parts))
            {
                parts = new List<Part>();
                quad._parts.Add(number, parts);
            }
            parts.Add(Part.LoadFrom(filePath));
        }
        
        return quad;
    }

    public Part GetTestPart()
    {
        return _parts.Values.First().First();
    }
}