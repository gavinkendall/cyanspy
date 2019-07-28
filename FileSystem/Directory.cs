using System;
using System.Collections.Generic;
using System.Text;

namespace cyanspy.FileSystem
{
    public class Directory
    {
        public string Name { get; set; }

        public Dictionary<string, File> Files { get; set; }

        public Dictionary<string, Directory> Directories { get; set; }

        public Directory()
        {
            Files = new Dictionary<string, File>();
            Directories = new Dictionary<string, Directory>();
        }
    }
}
