namespace Compiler.Utils.FileSystem;

public interface IFileReader
{
    string ReadAllText(string path);

    IEnumerable<string> ReadLines(string path);

    IEnumerable<string> EnumerateFolder(string path);

    string GetFullPath(string path);
}

public class FileReader : IFileReader
{
    public string ReadAllText(string path) => File.ReadAllText(path);

    public IEnumerable<string> ReadLines(string path) => File.ReadAllLines(path);

    public IEnumerable<string> EnumerateFolder(string path)
    {
        foreach (var filePath in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
        {
            yield return filePath;
        }
    }

    public string GetFullPath(string path)
    {
        return Path.GetFullPath(path);
    }
}
