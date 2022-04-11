using FileSystem.Interfaces;
using System.IO;

namespace FileSystem
{
    public class DefaultPathProvider : IPathProvider
    {
        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
