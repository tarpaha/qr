using SixLabors.ImageSharp;

namespace qr;

internal class Part
{
    public string Path { get; }
    public Image Image { get; }

    private Part(string path, Image image)
    {
        Path = path.Replace("\\", "/");
        Image = image;
    }

    public static Part LoadFrom(string path)
    {
        return new Part(path, Image.Load(path));
    }
}