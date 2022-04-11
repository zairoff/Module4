namespace FileSystem.Interfaces
{
    public interface IPathProvider
    {
        string GetFileName(string path);
        string GetDirectoryName(string path);
    }
}
