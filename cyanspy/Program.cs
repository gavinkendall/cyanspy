using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cyanspy
{
    public class Program
    {
        private static int HiringBudget = Cost.InitialHiringBudget;

        private static Dictionary<int, Spy> spyCollection = new Dictionary<int, Spy>();
        private static Dictionary<string, Location> locationCollection = new Dictionary<string, Location>();

        public static void Main()
        {
            Location hq = new Location("HQ");
            Location hotel = new Location("Hotel");
            Location garage = new Location("Garage");
            Location cafe = new Location("Cafe");

            locationCollection.Add(hq.Name, hq);
            locationCollection.Add(hotel.Name, hotel);
            locationCollection.Add(garage.Name, garage);
            locationCollection.Add(cafe.Name, cafe);
            
            string commandInput = string.Empty;

            do
            {
                Console.Write(DateTime.Now.ToString("HH:mm:ss") + " " + HiringBudget + "> ");
                commandInput = Console.ReadLine();

                Regex rgxCommandInput = new Regex(@"(?<Command>[a-z]+) ?(?<Value1>[0-9a-zA-Z]+)? ?(?<Value2>[0-9a-zA-Z]+)?");

                string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                string value1 = rgxCommandInput.Match(commandInput).Groups["Value1"].Value;
                string value2 = rgxCommandInput.Match(commandInput).Groups["Value2"].Value;

                switch (command.ToLower())
                {
                    case Command.Help:
                        Console.WriteLine("Command\t\t\tExplanation");
                        Console.WriteLine("hire\t\t\tHires a new spy");
                        Console.WriteLine("list\t\t\tLists hired spies");
                        Console.WriteLine("travel\t\t\tTravel status of hired spies");
                        Console.WriteLine("map\t\t\tShows a map of locations");
                        Console.WriteLine("move [spy] [location]\tMoves a spy to a new location");
                        Console.WriteLine("exit\t\t\tQuits the game");
                        break;

                    case Command.Move:
                        if (Int32.TryParse(value1, out int spyId))
                        {
                            if (spyCollection.ContainsKey(spyId))
                            {
                                if (locationCollection.ContainsKey(value2))
                                {
                                    Location destination = locationCollection[value2];

                                    TimeSpan ts = GetTimeSpanBetweenLocations(spyCollection[spyId].Source, destination);

                                    spyCollection[spyId].TravelStartTime = DateTime.Now;
                                    spyCollection[spyId].TravelEndTime = DateTime.Now.Add(ts);
                                    spyCollection[spyId].Destination = destination;

                                    Console.WriteLine("Spy " + spyId + " moving from " + spyCollection[spyId].Source.Name + " to " + spyCollection[spyId].Destination.Name);
                                }
                                else
                                {
                                    Console.WriteLine("No location of \"" + value2 + "\" could be found on the map");
                                }
                            }
                        }
                        break;

                    case Command.List:
                    case Command.ListShortcut:
                        Console.WriteLine("ID\tName\tCondition");

                        foreach (Spy spy in spyCollection.Values)
                        {
                            Console.WriteLine(spy.Id + "\t" + spy.Name +
                                "\t" + spy.Condition.Status + " (" + spy.HP + "%)");
                        }
                        break;

                    case Command.Map:
                    case Command.MapShortcut:
                        Console.WriteLine("Location");

                        foreach (Location location in locationCollection.Values)
                        {
                            Console.WriteLine(location.Name + " " + location.X + "," + location.Y);
                        }
                        break;

                    case Command.Travel:
                    case Command.TravelShortcut:
                        Console.WriteLine("Spy\tDestination\tTravel Status\t\tTravel Mode");
                        
                        foreach (Spy spy in spyCollection.Values)
                        {
                            if (spy.Destination != null)
                            {
                                Console.Write(spy.Id +
                                    ("\t" + spy.Destination.Name) + "\t\t");

                                    Console.Write(DateTime.Now >= spy.TravelEndTime ? "Arrived\t\t\t" : "On Route (" +
                                    spy.TravelEndTime.ToString("hh:mm:ss") + " ETA)\t");

                                Console.WriteLine();
                            }
                        }
                        break;

                    case Command.Hire:
                    case Command.HireShortcut:
                        if (HiringBudget >= Cost.HiringCostPerSpy)
                        {
                            Spy newSpy;

                            do
                            {
                                newSpy = new Spy();
                            }
                            while (spyCollection.ContainsKey(newSpy.Id));

                            newSpy.Source = locationCollection["HQ"];

                            spyCollection.Add(newSpy.Id, newSpy);

                            Console.WriteLine("Spy " + newSpy.Id + " has joined the organization!");

                            HiringBudget -= newSpy.HiringCost;
                        }
                        else
                        {
                            Console.WriteLine("There are no more funds in the hiring budget to hire a new spy!");
                        }
                        break;
                }
            }
            while (!commandInput.Equals(Command.Exit) && !commandInput.Equals(Command.ExitShortcut));
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