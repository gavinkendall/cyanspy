using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cyanspy
{
    public class Program
    {
        private static readonly string PLAYER_NAME = "cyan";
        private static readonly string PLAYER_MNEMONIC = "c";

        public static void Main()
        {
            try
            {
                Map map = new Map();
                Location location = new Location(PLAYER_NAME, PLAYER_MNEMONIC);

                map.Add(location);

                string commandInput = string.Empty;

                do
                {
                    if (map.Enabled)
                    {
                        map.Render();
                    }

                    Console.Write(Time.Show() + "> ");
                    commandInput = Console.ReadLine();

                    Regex rgxCommandInput = new Regex(@"(?<Command>[a-z]+) ?(?<Value1>[0-9a-zA-Z]+)? ?(?<Value2>[0-9a-zA-Z]+)?");

                    string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                    string value1 = rgxCommandInput.Match(commandInput).Groups["Value1"].Value;
                    string value2 = rgxCommandInput.Match(commandInput).Groups["Value2"].Value;

                    switch (command.ToLower())
                    {
                        case Command.Move:
                            Location playerLocation = map.GetLocationByName(PLAYER_NAME);

                            int x = playerLocation.X;
                            int y = playerLocation.Y;

                            if (Int32.TryParse(value1, out x) && Int32.TryParse(value2, out y))
                            {
                                if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                                {
                                    playerLocation.X = x;
                                    playerLocation.Y = y;
                                }
                            }
                            break;

                        case Command.Map:
                            if (value1.Equals("on"))
                            {
                                map.Enabled = true;
                            }

                            if (value1.Equals("off"))
                            {
                                map.Enabled = false;
                            }
                            break;

                        case Command.Time:
                            if (value1.Equals("on"))
                            {
                                Time.Enabled = true;
                            }

                            if (value1.Equals("off"))
                            {
                                Time.Enabled = false;
                            }
                            break;
                    }

                    Console.Clear();
                }
                while (!commandInput.Equals(Command.Exit));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        private static TimeSpan GetTimeSpanBetweenLocations(Location source, Location destination)
        {
            int hours = 0;
            int minutes = 0;
            int seconds = 0;

            if (source.X > destination.X)
            {
                minutes = source.X - destination.X;
            }

            if (destination.X > source.X)
            {
                minutes = destination.X - source.X;
            }

            if(source.Y > destination.Y)
            {
                minutes = source.Y - destination.Y;
            }

            if (destination.Y > source.Y)
            {
                minutes = destination.Y - source.Y;
            }

            return new TimeSpan(hours, minutes, seconds);
        }
    }
}