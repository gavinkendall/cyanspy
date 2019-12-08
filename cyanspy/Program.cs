using System;
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
                Map map = new Map();

                Mech mech = new Mech("Atomic Rabbit", "R");

                Location locationWater = new Location("water", "W");

                map.AddLocation(mech.Source);
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

                    Console.WriteLine("Mech Name = " + mech.Name);
                    Console.WriteLine("Mech Mnemoic = " + mech.Mnemonic);
                    Console.WriteLine("Mech Moving? " + mech.IsMoving.ToString());
                    Console.WriteLine("Mech Source = " + mech.Source.X + " " + mech.Source.Y);
                    Console.WriteLine("Mech Destination = " + mech.Destination.X + " " + mech.Destination.Y);

                    Location water = map.GetLocationByName("water");
                    Console.WriteLine("Water = " + water.X + " " + water.Y);
                    
                    if (map.AreLocationsWithinRange(mech.Source, water, range: 1))
                    {
                        Console.WriteLine("Mech within attack range of object");
                    }

                    if(map.AreLocationsWithinRange(mech.Source, water, range: 2))
                    {
                        Console.WriteLine("Mech within missile launch range of object");
                    }

                    if (map.AreLocationsWithinRange(mech.Source, water, range: 3))
                    {
                        Console.WriteLine("Mech within radar scan range of object");
                    }

                    TimeSpan ts = map.GetTimeSpanBetweenLocations(mech.Source, water);
                    Console.WriteLine("Duration between Mech and Water = " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds);

                    Console.WriteLine("Duration between Mech Source and Mech Destination = " + mech.TimeToDestination.Hours + ":" + mech.TimeToDestination.Minutes + ":" + mech.TimeToDestination.Seconds);

                    Console.WriteLine("Mech ETA to Destination = " + mech.EstimatedTimeOfArrival.ToString("HH:mm:ss"));

                    if (mech.DestinationReached())
                    {
                        map.RemoveLocation(mech.Source);
                        map.AddLocation(mech.Destination);
                        mech.Source = mech.Destination;
                    }

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
                            if (!mech.IsMoving)
                            {
                                if (Int32.TryParse(value1, out int x) && Int32.TryParse(value2, out int y))
                                {
                                    if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                                    {
                                        Location newMechLocation = new Location(mech.Name, mech.Mnemonic);
                                        newMechLocation.X = x;
                                        newMechLocation.Y = y;

                                        TimeSpan timeToDestination = map.GetTimeSpanBetweenLocations(mech.Source, newMechLocation);

                                        mech.Move(newMechLocation, timeToDestination);
                                    }
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