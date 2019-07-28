using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cyanspy
{
    public class Program
    {
        private static Network.User currentUser;
        private static Network.Server currentServer;
        private static FileSystem.Directory currentDirectory;

        public static void Main()
        {
            currentUser = new Network.User() { Name = "cyan" };
            currentServer = new Network.Server() { Name = "admin" };

            currentServer.Directories.Add("sys", new FileSystem.Directory() { Name = "sys" });
            currentDirectory = currentServer.Directories["sys"];

            currentServer.Directories["sys"].Files = new Dictionary<string, FileSystem.File>();
            currentServer.Directories["sys"].Directories = new Dictionary<string, FileSystem.Directory>();

            currentServer.Directories["sys"].Directories.Add("apps", new FileSystem.Directory() { Name = "apps" });
            currentServer.Directories["sys"].Directories.Add("users", new FileSystem.Directory() { Name = "users" });

            string commandInput = string.Empty;

            do
            {
                Console.Write("(" + currentServer.Name + ") " + DateTime.Now.ToString("HH:mm:ss") + " " + currentUser.Name + " /" + currentDirectory.Name + "> ");
                commandInput = Console.ReadLine();

                Regex rgxCommandInput = new Regex(@"(?<Command>[a-z]+) ?(?<Value>[a-z\.]+)?");

                string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                string value = rgxCommandInput.Match(commandInput).Groups["Value"].Value;

                switch(command)
                {
                    case Command.ChangeDirectory:
                        currentDirectory = ChangeDirectory(currentDirectory, new FileSystem.Directory() { Name = value });
                        break;

                    case Command.MakeDirectory:
                        MakeDirectory(value);
                        break;

                    case Command.List:
                        List();
                        break;

                    case Command.ListDirectory:
                        ListDirectory();
                        break;

                    case Command.ListFile:
                        ListFile(value);
                        break;
                }
            }
            while (!commandInput.Equals("exit"));
        }

        private static FileSystem.Directory ChangeDirectory(FileSystem.Directory currentDirectory, FileSystem.Directory directoryChangingTo)
        {
            FileSystem.Directory chosenDirectory = new FileSystem.Directory();

            if (directoryChangingTo.Name.Equals(".."))
            {
                chosenDirectory = currentServer.Directories["sys"];
            }
            else
            {
                if (currentServer.Directories.TryGetValue(currentDirectory.Name, out currentDirectory))
                {
                    if (currentDirectory.Directories.TryGetValue(directoryChangingTo.Name, out directoryChangingTo))
                    {
                        chosenDirectory = directoryChangingTo;
                    }
                }
            }

            return chosenDirectory;
        }

        private static void List()
        {
            ListDirectory();
            ListFile();
        }

        private static void ListDirectory()
        {
            ListDirectory(currentDirectory.Name);
        }

        private static void ListDirectory(string directoryName)
        {
            FileSystem.Directory chosenDirectory;

            if (currentServer.Directories.TryGetValue(directoryName, out chosenDirectory))
            {
                foreach (FileSystem.Directory directory in chosenDirectory.Directories.Values)
                {
                    Console.Write("/" + directory.Name + " ");
                }

                Console.WriteLine("\n" + chosenDirectory.Directories.Values.Count + " directories");
            }
        }

        private static void ListFile()
        {
            ListFile(currentDirectory.Name);
        }

        private static void ListFile(string directoryName)
        {
            FileSystem.Directory chosenDirectory;
            
            if (currentServer.Directories.TryGetValue(directoryName, out chosenDirectory))
            {
                foreach (FileSystem.File file in chosenDirectory.Files.Values)
                {
                    Console.Write(file.Name + " ");
                }

                Console.WriteLine("\n" + chosenDirectory.Files.Values.Count + " files");
            }
        }

        private static void MakeDirectory(string newDirectoryName)
        {
            MakeDirectory(currentDirectory, newDirectoryName);
        }

        private static void MakeDirectory(FileSystem.Directory chosenDirectory, string newDirectoryName)
        {
            if (currentServer.Directories.TryGetValue(chosenDirectory.Name, out chosenDirectory))
            {
                FileSystem.Directory newDirectory = new FileSystem.Directory { Name = newDirectoryName };

                chosenDirectory.Directories.Add(newDirectoryName, newDirectory);
            }
        }

        //private static void MakeFile(string directoryName, string name)
        //{
        //    FileSystem.File newFile = new FileSystem.File { Name = name };
        //    currentServer.Directories[directoryName].Files.Add(name, newFile);
        //}
    }
}