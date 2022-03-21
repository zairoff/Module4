// See https://aka.ms/new-console-template for more information
using FileSystem;

var fileSystem = new FileSystemVisitor(
    @"C:\Basics\Module4\FileSystem",
    f => f.Contains(".cs"), 
    new DefaultFileSystemProvider());

fileSystem.Started += (sender, args) => System.Console.WriteLine("Started");
fileSystem.Finished += (sender, args) => System.Console.WriteLine("Finished");

fileSystem.DirectoryFound += (sender, args) => 
    System.Console.WriteLine(
        $"DirectoryFound: {args.Name}" +
        $" \nType: {args.ItemType}" +
        $" \nPath: {args.Path} " +
        $"\nExclude: {args.Exclude}" +
        $"\nStop: {args.Stop}");

fileSystem.FilteredDirectoryFound += (sender, args) => 
    System.Console.WriteLine(
        $"FilteredDirectoryFound: {args.Name} " +
        $"\nType: {args.ItemType}" +
        $"\nPath: {args.Path}" +
        $"\nExclude: {args.Exclude}" +
        $"\nStop: {args.Stop}");

fileSystem.FileFound += (sender, args) => 
{
    if (args.Name.Contains(".txt"))
        args.Exclude = true;

    if (args.Name.Contains(".cs"))
        args.Stop = true;

    System.Console.WriteLine(
        $"FileFound: {args.Name} " +
        $"\nType: {args.ItemType}" +
        $"\nPath: {args.Path}" +
        $"\nExclude: {args.Exclude}" +
        $"\nStop: {args.Stop}");
};    

fileSystem.FilteredFileFound += (sender, args) =>
    {      
        System.Console.WriteLine($"FilteredFileFound: {args.Name}" +
            $"\nType: {args.ItemType}" +
            $"\nPath: {args.Path}" +
            $"\nExclude: {args.Exclude}" +
            $"\nStop: {args.Stop}");
    };

var items = fileSystem.FindAllFileAndDirectories();
//foreach (var item in items)
//    Console.WriteLine($"res: {item}");
