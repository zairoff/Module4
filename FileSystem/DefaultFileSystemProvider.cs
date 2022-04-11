using FileSystem.Interfaces;
using System.Collections.Generic;
using System.IO;

namespace FileSystem
{
    public class DefaultFileSystemProvider : IFileSystemProvider
    {
        public IEnumerable<string> GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public string GetDirectoryName(string path)
        {
            return Path.GetDirectoryName(path);
        }

        public IEnumerable<string> GetDirectories(string path)
        {
            return Directory.GetDirectories(path);
        }

        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
