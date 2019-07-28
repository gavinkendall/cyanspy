using System;
using System.Collections.Generic;
using System.Text;

namespace cyanspy.Network
{
    public class Server
    {
        public string Name { get; set; }

        public Dictionary<string, FileSystem.Directory> Directories { get; set; }

        public Dictionary<string, User> Users { get; set; }

        public Server()
        {
            Directories = new Dictionary<string, FileSystem.Directory>();
            Users = new Dictionary<string, User>();
        }
    }
}
