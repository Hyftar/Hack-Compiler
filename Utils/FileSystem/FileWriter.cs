namespace Compiler.Utils.FileSystem;

public interface IFileWriter
{
    void WriteToFile(string path, string contents);

    void DeleteIfExists(string path);

    void CreateDirectory(string path);
}

public class FileWriter : IFileWriter
{
    public void WriteToFile(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }

    public void DeleteIfExists(string path)
    {
        File.Delete(path);
    }

    public void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }
}
