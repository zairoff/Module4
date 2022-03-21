using System;
using FileSystem.Interfaces;
using System.Collections.Generic;

namespace FileSystem
{
    public class FileSystemVisitor
    {
        private readonly string _path;
        private readonly IFileSystemProvider _fileSystemProvider;
        private readonly Predicate<string> _filter;
        public event EventHandler Started;
        public event EventHandler Finished;
        public EventHandler<ItemEventArgs> FileFound;
        public EventHandler<ItemEventArgs> DirectoryFound;
        public EventHandler<ItemEventArgs> FilteredDirectoryFound;
        public EventHandler<ItemEventArgs> FilteredFileFound;

        public FileSystemVisitor(
            string path,
            Predicate<string> filter = null,
            IFileSystemProvider fileSystemProvider = null)
        {
            _path = path;
            _filter = filter;
            _fileSystemProvider = fileSystemProvider;
        }
        public IEnumerable<string> FindAllFileAndDirectories()
        {
            OnStarted();

            var items = FindAllFileAndDirectories(_path);
            foreach(var item in items)
            {
                yield return item;
            }            

            OnFinished();
        }

        private IEnumerable<string> FindAllFileAndDirectories(string path)
        {
            foreach (var directory in _fileSystemProvider.GetDirectories(_path))
            {
                OnDirectoryFound(directory);

                if (_filter(directory))
                    OnFilteredDirectoryFound(directory);

                yield return directory;
            }

            foreach (var file in _fileSystemProvider.GetFiles(_path))
            {
                var args = OnFileFound(file);

                if (_filter(file))
                    OnFilteredFileFound(file);

                if (args.Exclude)
                    continue;

                if (args.Stop)
                    yield break;

                yield return file;
            }
        }

        private void OnStarted()
        {
            Started?.Invoke(this, EventArgs.Empty);
        }
        
        private void OnFinished()
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private ItemEventArgs OnDirectoryFound(string directoryPath)
        {
            var args = new ItemEventArgs
            {
                Path = directoryPath,
                Name = _fileSystemProvider.GetDirectoryName(directoryPath),
                ItemType = typeof(ItemEventArgs),
            };

            DirectoryFound?.Invoke(this, args);

            return args;
        }

        private ItemEventArgs OnFilteredDirectoryFound(string directoryPath)
        {
            var args = new ItemEventArgs
            {
                Path = directoryPath,
                Name = _fileSystemProvider.GetDirectoryName(directoryPath),
                ItemType = typeof(ItemEventArgs),
            };

            FilteredDirectoryFound?.Invoke(this, args);

            return args;
        }

        private ItemEventArgs OnFileFound(string filPath)
        {
            var args = new ItemEventArgs
            {
                Path = filPath,
                Name = _fileSystemProvider.GetFileName(filPath),
                ItemType = typeof(ItemEventArgs),
            };

            FileFound?.Invoke(this, args);

            return args;
        }
        
        private ItemEventArgs OnFilteredFileFound(string filPath)
        {
            var args = new ItemEventArgs
            {
                Path = filPath,
                Name = _fileSystemProvider.GetFileName(filPath),
                ItemType = typeof(ItemEventArgs),
            };

            FilteredFileFound?.Invoke(this, args);

            return args;
        }
    }
}
