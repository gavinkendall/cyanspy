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
            locationCollection.Add("Boston", new Location("Boston", new TimeSpan(0, 1, 0)));
            locationCollection.Add("London", new Location("London", new TimeSpan(0, 2, 0)));
            locationCollection.Add("Paris", new Location("Paris", new TimeSpan(0, 3, 0)));
            locationCollection.Add("Tokyo", new Location("Tokyo", new TimeSpan(0, 5, 0)));

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
                        Console.WriteLine("deploy [spy] [location]\tDeploys a spy to a location");
                        Console.WriteLine("exit\t\t\tQuits the game\n");
                        Console.WriteLine("Example Deployment Commands");
                        Console.WriteLine("deploy 32 Paris\t\tDeploys Spy ID 32 to Paris");
                        Console.WriteLine("deploy 53 London\tDeploys Spy ID 53 to London");
                        break;

                    case Command.List:
                    case Command.ListShortcut:
                        Console.WriteLine("ID\tName\t\tCondition");

                        foreach (Spy spy in spyCollection.Values)
                        {
                            Console.WriteLine(spy.Id + "\t" + spy.Name + "\t" + spy.Condition + " (" + spy.HP + "%)");
                        }
                        break;

                    case Command.Map:
                    case Command.MapShortcut:
                        Console.WriteLine("Location\tFlight Duration");

                        foreach (Location location in locationCollection.Values)
                        {
                            Console.WriteLine(location.Name + "\t\t" +
                                location.FlightDuration.Hours + ":" +
                                location.FlightDuration.Minutes + ":" +
                                location.FlightDuration.Seconds);
                        }
                        break;

                    case Command.Travel:
                    case Command.TravelShortcut:
                        Console.WriteLine("Spy\tDestination\tTravel Status\t\tCondition");

                        foreach (Spy spy in spyCollection.Values)
                        {
                            Console.Write(spy.Id +
                                (spy.Deployed ? "\t" + spy.Destination : string.Empty) + "\t\t");

                            if (spy.Deployed)
                            {
                                Console.Write(DateTime.Now >= spy.TravelEndTime ? "Arrived\t\t\t" : "On Route (" +
                                spy.TravelEndTime.ToString("hh:mm:ss") + " ETA)\t");
                            }
                            else
                            {
                                Console.Write("\t\t\t\t");
                            }

                            Console.Write(spy.Condition);
                            Console.Write(" (" + spy.HP + "%)");

                            Console.WriteLine();
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

                            spyCollection.Add(newSpy.Id, newSpy);

                            Console.WriteLine("Spy " + newSpy.Id + " has joined the organization!");

                            HiringBudget -= newSpy.HiringCost;
                        }
                        else
                        {
                            Console.WriteLine("There are no more funds in the hiring budget to hire a new spy!");
                        }
                        break;

                    case Command.Deploy:
                    case Command.DeployShortcut:
                        if (Int32.TryParse(value1, out int spyId))
                        {
                            if (spyCollection.ContainsKey(spyId))
                            {
                                string location = value2;

                                if (locationCollection.ContainsKey(location))
                                {
                                    if (!spyCollection[spyId].Deployed)
                                    {
                                        Console.WriteLine("Spy " + spyId + " deployed to " + location);

                                        spyCollection[spyId].Deployed = true;
                                        spyCollection[spyId].TravelStartTime = DateTime.Now;
                                        spyCollection[spyId].TravelEndTime = DateTime.Now.Add(locationCollection[location].FlightDuration);
                                        spyCollection[spyId].Destination = locationCollection[location].Name;
                                        spyCollection[spyId].Location = locationCollection[location];
                                    }
                                    else
                                    {
                                        Console.WriteLine("Spy " + spyId + " has already been deployed");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No location of \"" + location + "\" could be found on the map");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No spy of ID \"" + spyId + "\" could be found in the organization's hiring roster");
                            }
                        }
                        break;
                }
            }
            while (!commandInput.Equals(Command.Exit) && !commandInput.Equals(Command.ExitShortcut));
        }
    }
}