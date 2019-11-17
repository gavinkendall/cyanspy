using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cyanspy
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                Reset:
                Pilot cyan = new Pilot("Cyan");

                Mech playerMech = new Mech("PlayerMech");
                Dictionary<string, Pilot> playerMechPilots = new Dictionary<string, Pilot>();

                playerMechPilots.Add(cyan.Name, cyan);

                playerMech.Pilots = playerMechPilots;

                Map map = new Map();
                Location player = new Location(playerMech.Name, playerMech.Mnemonic);
                Location water = new Location("water", "W");

                map.AddLocation(player);
                map.AddLocation(water);

                string commandInput = string.Empty;

                // Setup just for development/debugging.
                Time.Enabled = true;
                map.Enabled = true;

                do
                {
                    if (map.Enabled)
                    {
                        map.Render();
                    }

                    Location pl = map.GetLocationByName("PlayerMech");
                    Console.WriteLine("Player Mech = " + pl.X + " " + pl.Y);

                    Location waterLocation = map.GetLocationByName("water");
                    Console.WriteLine("Water = " + waterLocation.X + " " + waterLocation.Y);

                    TimeSpan ts = GetTimeSpanBetweenLocations(pl, waterLocation);
                    Console.WriteLine("Duration between Player Mech and Water = " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds);

                    Console.Write(Time.Show() + "> ");
                    commandInput = Console.ReadLine();

                    Regex rgxCommandInput = new Regex(@"(?<Command>[a-z]+) ?(?<Value1>[0-9a-zA-Z]+)? ?(?<Value2>[0-9a-zA-Z]+)?");

                    string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                    string value1 = rgxCommandInput.Match(commandInput).Groups["Value1"].Value;
                    string value2 = rgxCommandInput.Match(commandInput).Groups["Value2"].Value;

                    switch (command.ToLower())
                    {
                        case "reset":
                            Console.Clear();
                            goto Reset;
                            break;

                        case Command.Move:
                            Location playerLocation = map.GetLocationByName("PlayerMech");

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