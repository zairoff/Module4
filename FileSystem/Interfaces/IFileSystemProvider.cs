using System;
using System.Collections.Generic;
using System.Text;

namespace FileSystem.Interfaces
{
    public interface IFileSystemProvider
    {
        IEnumerable<string> GetDirectories(string path);
        IEnumerable<string> GetFiles(string path);
        string GetFileName(string path);
        string GetDirectoryName(string path);
    }
}
