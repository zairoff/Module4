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
        public event EventHandler<ItemEventArgs> FileFound;
        public event EventHandler<ItemEventArgs> DirectoryFound;
        public event EventHandler<ItemEventArgs> FilteredDirectoryFound;
        public event EventHandler<ItemEventArgs> FilteredFileFound;

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
            var directories = _fileSystemProvider.GetDirectories(path);
            foreach (var directory in directories)
            {
                var directoryArgs = OnDirectoryFound(directory);
                var filteredDirectoryArgs = GetFilteredDirectoryArgs(directory);

                var isExcluded = IsExcluded(directoryArgs, filteredDirectoryArgs);

                if (isExcluded)
                    continue;

                var shouldStop = ShouldStop(directoryArgs, filteredDirectoryArgs);

                if(shouldStop)
                    yield break;

                yield return directory;

                var files = _fileSystemProvider.GetFiles(directory);

                foreach (var file in files)
                {
                    var fileArgs = OnFileFound(file);
                    var filteredFileArgs = GetFilteredFileArgs(file);

                    isExcluded = IsExcluded(fileArgs, filteredFileArgs);

                    if (isExcluded)
                        continue;

                    shouldStop = ShouldStop(fileArgs, filteredFileArgs);

                    if (shouldStop)
                        yield break;

                    yield return file;
                }
            }            
        }

        private bool ShouldStop(ItemEventArgs args, ItemEventArgs filteredArgs)
        {
            if (filteredArgs == null) return false;

            return args.Stop || filteredArgs.Stop;
        }

        private bool IsExcluded(ItemEventArgs args, ItemEventArgs filteredArgs)
        {
            if (filteredArgs == null) return false;

            return args.Exclude || filteredArgs.Exclude;
        }

        private ItemEventArgs GetFilteredFileArgs(string filePath)
        {
            if (_filter == null) return null;

            var isFiltered = _filter(filePath);

            return isFiltered ? OnFilteredFileFound(filePath) : null;
        }

        private ItemEventArgs GetFilteredDirectoryArgs(string directoryPath)
        {
            if (_filter == null) return null;

            var isFiltered = _filter(directoryPath);

            return isFiltered ? OnFilteredDirectoryFound(directoryPath) : null;
        }

        private void OnStarted()
        {
            Started?.Invoke(this, ItemEventArgs.Empty);
        }
        
        private void OnFinished()
        {
            Finished?.Invoke(this, ItemEventArgs.Empty);
        }

        private ItemEventArgs OnDirectoryFound(string directoryPath)
        {
            var args = new ItemEventArgs
            {
                Path = directoryPath,
                Name = _fileSystemProvider.GetDirectoryName(directoryPath),
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
            };

            FilteredFileFound?.Invoke(this, args);

            return args;
        }
    }
}
