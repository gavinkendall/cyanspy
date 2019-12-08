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

                Mech mechAtomicRabbit = new Mech("Atomic Rabbit", "R");
                Mech mechCosmicWolf = new Mech("Cosmic Wolf", "W");

                map.AddLocation(mechAtomicRabbit.Position);
                map.AddLocation(mechCosmicWolf.Position);

                string commandInput = string.Empty;

                // Setup just for development/debugging.
                Time.Enabled = true;
                map.Enabled = true;

                do
                {
                    map.GetLocationByName(mechAtomicRabbit.Name).X = mechAtomicRabbit.Position.X;
                    map.GetLocationByName(mechAtomicRabbit.Name).Y = mechAtomicRabbit.Position.Y;

                    if (map.Enabled)
                    {
                        map.Render();
                    }

                    Console.WriteLine("Mech Name = " + mechAtomicRabbit.Name);
                    Console.WriteLine("Mech Mnemonic = " + mechAtomicRabbit.Mnemonic);
                    Console.WriteLine("Mech Moving = " + mechAtomicRabbit.IsMoving.ToString());
                    Console.WriteLine("Mech Position = " + mechAtomicRabbit.Position.X + " " + mechAtomicRabbit.Position.Y);

                    if (mechAtomicRabbit.IsMoving)
                    {
                        Console.WriteLine("Mech Destination = " + mechAtomicRabbit.Destination.X + " " + mechAtomicRabbit.Destination.Y);
                    }

                    if (map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 1))
                    {
                        Console.WriteLine("Mech within attack range of enemy");
                    }

                    if(map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 2))
                    {
                        Console.WriteLine("Mech within missile launch range of enemy");
                    }

                    if (map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 3))
                    {
                        Console.WriteLine("Mech within radar scan range of enemy");
                    }

                    //TimeSpan ts = map.GetTimeSpanBetweenLocations(mech.Position, water);
                    //Console.WriteLine("Duration between Mech and Water = " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds);

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
                            if (!mechAtomicRabbit.IsMoving)
                            {
                                if (Int32.TryParse(value1, out int x) && Int32.TryParse(value2, out int y))
                                {
                                    if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                                    {
                                        Location newMechLocation = new Location(mechAtomicRabbit.Name, mechAtomicRabbit.Mnemonic);
                                        newMechLocation.X = x;
                                        newMechLocation.Y = y;

                                        mechAtomicRabbit.Move(newMechLocation);
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