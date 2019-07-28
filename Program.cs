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

            FileSystem.Directory sysDirectory = new FileSystem.Directory() { Name = "sys", IsRoot = true };

            currentServer.Directories.Add(sysDirectory.Name, sysDirectory);
            currentDirectory = sysDirectory;

            currentServer.Directories[sysDirectory.Name].Files = new Dictionary<string, FileSystem.File>();
            currentServer.Directories[sysDirectory.Name].Directories = new Dictionary<string, FileSystem.Directory>();

            FileSystem.Directory appsDirectory = new FileSystem.Directory() { Name = "apps", IsRoot = false, ParentDirectory = sysDirectory };
            FileSystem.Directory usersDirectory = new FileSystem.Directory() { Name = "users", IsRoot = false, ParentDirectory = sysDirectory };

            currentServer.Directories[sysDirectory.Name].Directories.Add(appsDirectory.Name, appsDirectory);
            currentServer.Directories[sysDirectory.Name].Directories.Add(usersDirectory.Name, usersDirectory);

            string commandInput = string.Empty;

            do
            {
                Console.Write("(" + currentServer.Name + ") " + DateTime.Now.ToString("HH:mm:ss") + " " + currentUser.Name + " /" + currentDirectory.Name + "> ");
                commandInput = Console.ReadLine();

                Regex rgxCommandInput = new Regex(@"(?<Command>[a-zA-Z]+) ?(?<Value>[a-zA-Z\.]+)?");

                string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                string value = rgxCommandInput.Match(commandInput).Groups["Value"].Value;

                switch(command.ToLower())
                {
                    case Command.ChangeDirectory:
                        FileSystem.Directory directoryChangingTo = ChangeDirectory(currentDirectory, new FileSystem.Directory() { Name = value });

                        if (directoryChangingTo != null)
                        {
                            currentDirectory = directoryChangingTo;
                        }
                        break;

                    case Command.MakeDirectory:
                        MakeDirectory(currentDirectory, value);
                        break;

                    case Command.MakeFile:
                        MakeFile(currentDirectory, value);
                        break;

                    case Command.List:
                        List();
                        break;

                    case Command.ListDirectory:
                        ListDirectory();
                        break;

                    case Command.ListFile:
                        ListFile();
                        break;

                    case Command.Remove:
                        Remove(value);
                        break;

                    case Command.RemoveDirectory:
                        RemoveDirectory(value);
                        break;

                    case Command.RemoveFile:
                        RemoveFile(value);
                        break;
                }
            }
            while (!commandInput.Equals("exit"));
        }

        private static FileSystem.Directory ChangeDirectory(FileSystem.Directory currentDirectory, FileSystem.Directory directoryChangingTo)
        {
            if (directoryChangingTo.Name.Equals(".."))
            {
                return currentDirectory.ParentDirectory;
            }

            if (currentDirectory.Directories.TryGetValue(directoryChangingTo.Name, out directoryChangingTo))
            {
                return directoryChangingTo;
            }

            return null;
        }

        private static void List()
        {
            ListDirectory();
            ListFile();
        }

        private static void ListDirectory()
        {
            foreach (FileSystem.Directory directory in currentDirectory.Directories.Values)
            {
                Console.Write("/" + directory.Name + " ");
            }

            Console.WriteLine("\nDirectories: " + currentDirectory.Directories.Values.Count);
        }

        private static void ListFile()
        {
            foreach (FileSystem.File file in currentDirectory.Files.Values)
            {
                Console.Write(file.Name + " ");
            }

            Console.WriteLine("\nFiles: " + currentDirectory.Files.Values.Count);
        }

        private static void MakeDirectory(FileSystem.Directory currentDirectory, string newDirectoryName)
        {
            if (newDirectoryName.Length > 8)
            {
                newDirectoryName = newDirectoryName.Substring(0, 8);
            }

            if (!currentDirectory.Directories.ContainsKey(newDirectoryName))
            {
                FileSystem.Directory newDirectory = new FileSystem.Directory { Name = newDirectoryName, IsRoot = false, ParentDirectory = currentDirectory };

                currentDirectory.Directories.Add(newDirectory.Name, newDirectory);
                Console.WriteLine("Directory \"" + newDirectory.Name + "\" created");
            }
        }

        private static void MakeFile(FileSystem.Directory currentDirectory, string newFileName)
        {
            if (newFileName.Length > 8)
            {
                newFileName = newFileName.Substring(0, 8);
            }

            if (!currentDirectory.Files.ContainsKey(newFileName))
            {
                FileSystem.File newFile = new FileSystem.File { Name = newFileName };

                currentDirectory.Files.Add(newFile.Name, newFile);
                Console.WriteLine("File \"" + newFile.Name + "\" created");
            }
        }

        private static void Remove(string name)
        {
            RemoveDirectory(name);
            RemoveFile(name);
        }

        private static void RemoveDirectory(string directoryName)
        {
            if (currentDirectory.Directories.ContainsKey(directoryName))
            {
                currentDirectory.Directories.Remove(directoryName);
                Console.WriteLine("Directory \"" + directoryName + "\" removed");
            }
        }

        private static void RemoveFile(string fileName)
        {
            if (currentDirectory.Files.ContainsKey(fileName))
            {
                currentDirectory.Files.Remove(fileName);
                Console.WriteLine("File \"" + fileName + "\" removed");
            }
        }
    }
}