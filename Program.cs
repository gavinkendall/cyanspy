using System;

namespace cyanspy
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string commandInput = string.Empty;

            do
            {
                Console.Write("> ");
                commandInput = Console.ReadLine();
            }
            while (!commandInput.Equals("exit"));
        }
    }
}