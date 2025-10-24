namespace NameSorter.Console.Features.SortName;

public interface IFileManager
{
    bool Exists(string path);
    void Write(string path, IEnumerable<string> content);
    IEnumerable<string> ReadAllLines(string path);
}

public sealed class FileManager : IFileManager
{
    public bool Exists(string path) => File.Exists(path);
    public void Write(string path, IEnumerable<string> content) => File.WriteAllLines(path, content);
    public IEnumerable<string> ReadAllLines(string path) => File.ReadAllLines(path);
}