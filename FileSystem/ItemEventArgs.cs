using System;

namespace FileSystem
{
    public class ItemEventArgs : EventArgs
    {
        public Type ItemType { get; set; }

        public string Path { get; set; }

        public string Name { get; set; }

        public bool Exclude { get; set; }

        public bool Stop { get; set; }
    }
}
