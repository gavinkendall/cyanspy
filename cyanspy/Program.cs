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
                Mech mechLionheart = new Mech("Lionheart");

                Pilot pilotCyan = new Pilot("Cyan");
                Pilot pilotAeon = new Pilot("Aeon");

                Dictionary<string, Pilot> mechLionHeartPilots = new Dictionary<string, Pilot>
                {
                    { pilotAeon.Name, pilotAeon },
                    { pilotCyan.Name, pilotCyan }
                };

                mechLionheart.Pilots = mechLionHeartPilots;

                Map map = new Map();

                Location locationMechLionheart = new Location(mechLionheart.Name, mechLionheart.Mnemonic);
                Location locationWater = new Location("water", "W");

                map.AddLocation(locationMechLionheart);
                map.AddLocation(locationWater);

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

                    Location mechLocation = map.GetLocationByName("Lionheart");
                    Console.WriteLine("Mech = " + mechLocation.X + " " + mechLocation.Y);

                    Location waterLocation = map.GetLocationByName("water");
                    Console.WriteLine("Water = " + waterLocation.X + " " + waterLocation.Y);
                    
                    if (map.AreLocationsWithinRange(mechLocation, waterLocation, range: 1))
                    {
                        Console.WriteLine("Mech within attack range of object");
                    }

                    if(map.AreLocationsWithinRange(mechLocation, waterLocation, range: 2))
                    {
                        Console.WriteLine("Mech within missile launch range of object");
                    }

                    if (map.AreLocationsWithinRange(mechLocation, waterLocation, range: 3))
                    {
                        Console.WriteLine("Mech within radar scan range of object");
                    }

                    TimeSpan ts = map.GetTimeSpanBetweenLocations(mechLocation, waterLocation);
                    Console.WriteLine("Duration between Mech and Water = " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds);

                    Console.Write(Time.Show() + "> ");
                    commandInput = Console.ReadLine();

                    Regex rgxCommandInput = new Regex(@"(?<Command>[a-z]+) ?(?<Value1>[0-9a-zA-Z]+)? ?(?<Value2>[0-9a-zA-Z]+)?");

                    string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                    string value1 = rgxCommandInput.Match(commandInput).Groups["Value1"].Value;
                    string value2 = rgxCommandInput.Match(commandInput).Groups["Value2"].Value;

                    switch (command.ToLower())
                    {
                        // For debugging purposes only.
                        case "reset":
                            Console.Clear();
                            goto Reset;
                            break;

                        case Command.Move:
                            Location playerLocation = map.GetLocationByName("Lionheart");

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
    }
}