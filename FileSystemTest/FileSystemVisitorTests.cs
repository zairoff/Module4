using FileSystem;
using FileSystem.Interfaces;
using Moq;
using System.Collections.Generic;
using System.Linq;
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
            var eventsCount = 6;
            var events = new List<ItemEventArgs>();
            var fakePath = "fake path";

            var fileSystemVisitor = new FileSystemVisitor(
                                    fakePath,
                                    path => path.Contains("fake"), 
                                    _fileSystemProviderMock.Object);

            _fileSystemProviderMock.Setup(f => f.GetDirectories(fakePath)).Returns(_directories);
            _fileSystemProviderMock.Setup(f => f.GetFiles(fakePath)).Returns(_files);
            _fileSystemProviderMock.Setup(f => f.GetDirectoryName(_directories[0])).Returns(_directories[0]);
            _fileSystemProviderMock.Setup(f => f.GetFileName(_files[0])).Returns(_files[0]);

            fileSystemVisitor.Started += (s, e) => { events.Add(new ItemEventArgs()); };
            fileSystemVisitor.Finished += (s, e) => { events.Add(new ItemEventArgs()); };

            fileSystemVisitor.FileFound += (s, e) => { events.Add(e); };
            fileSystemVisitor.DirectoryFound += (s, e) => { events.Add(e); };

            var result = fileSystemVisitor.FindAllFileAndDirectories().ToList();

            Assert.Equal(eventsCount, events.Count);
            Assert.Equal(_directories[0], events[1].Name);
            Assert.Equal(_directories[0], result[0]);

        }
    }
}