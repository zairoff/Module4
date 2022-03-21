using FileSystem;
using FileSystem.Interfaces;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace FileSystemTest
{
    public class FileSystemVisitorTests
    {
        private readonly Mock<IFileSystemProvider> _fileSystemProviderMock;
        private readonly List<string> _directories = new List<string> { "Directory1", "Directory2" };
        private readonly List<string> _files = new List<string> { "File1", "File2" };

        public FileSystemVisitorTests()
        {
            _fileSystemProviderMock = new Mock<IFileSystemProvider>();
        }

        [Fact]
        public void GetAllFileAndDirectories_Should_Raise_DefinedEvents()
        {
            var events = new List<ItemEventArgs>();

            var fileSystemVisitor = new FileSystemVisitor(
                                    "fake path",
                                    path => path.Contains("fake"), 
                                    _fileSystemProviderMock.Object);

            _fileSystemProviderMock.Setup(f => f.GetDirectories("")).Returns(_directories);
            _fileSystemProviderMock.Setup(f => f.GetFiles("")).Returns(_files);
            _fileSystemProviderMock.Setup(f => f.GetDirectoryName("")).Returns(_directories[0]);
            _fileSystemProviderMock.Setup(f => f.GetFileName("")).Returns(_files[0]);

            fileSystemVisitor.Started += (s, e) => { events.Add(new ItemEventArgs()); };
            fileSystemVisitor.Finished += (s, e) => { events.Add(new ItemEventArgs()); };

            fileSystemVisitor.FileFound += (s, e) => { events.Add(e); };
            fileSystemVisitor.DirectoryFound += (s, e) => { events.Add(e); };

            var result = fileSystemVisitor.FindAllFileAndDirectories();

        }
    }
}