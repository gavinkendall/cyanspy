using System;
using System.Timers;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cyanspy
{
    public class Mech
    {
        // A mech has applications, components, and weapons.
        public Dictionary<string, Application> Applications { get; set; }
        public Dictionary<string, Component> Components { get; set; }
        public Dictionary<string, Weapon> Weapons { get; set; }

        private string _status = string.Empty;
        public static string _systemMessage = string.Empty;

        public string Name { get; set; }
        public string Mnemonic { get; private set; }
        public Location Destination { get; set; }
        public Location Position { get; set; }
        public bool IsMoving { get; set; }
        public int MovementSpeed { get; set; }

        public bool ShowInfo { get; set; }

        private Timer _movementTimer;

        public Mech(string name, string mnemonic)
        {
            Name = name;
            Mnemonic = mnemonic;

            Position = new Location(Name, Mnemonic);

            _status = "Idle";
            IsMoving = false;

            MovementSpeed = 10000;
            _movementTimer = new Timer();
            _movementTimer.Elapsed += _movementTimer_Elapsed;
        }

        public void InitiateCommandInterface(Map map)
        {
            string commandInput = string.Empty;

            do
            {
                map.GetLocationByName(Name).X = Position.X;
                map.GetLocationByName(Name).Y = Position.Y;

                map.Render();

                Info();

                if (!string.IsNullOrEmpty(_systemMessage))
                {
                    Console.WriteLine("System Message: " + _systemMessage);
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
                        if (IsMoving)
                        {
                            _systemMessage = "Command failed. Already moving.";
                        }
                        else
                        {
                            if (Int32.TryParse(value1, out int x) && Int32.TryParse(value2, out int y))
                            {
                                if (x >= 0 && x <= 9 && y >= 0 && y <= 9)
                                {
                                    Location newMechLocation = new Location(Name, Mnemonic);
                                    newMechLocation.X = x;
                                    newMechLocation.Y = y;

                                    Move(newMechLocation);
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

                    case Command.Info:
                        if (value1.Equals("on"))
                        {
                            ShowInfo = true;
                        }

                        if (value1.Equals("off"))
                        {
                            ShowInfo = false;
                        }
                        break;
                }

                Console.Clear();
            }
            while (!commandInput.Equals(Command.Exit));
        }

        public void Info()
        {
            if (!ShowInfo) return;

            Console.WriteLine("Status: " + _status);
            Console.WriteLine("Position: " + Position.X + " " + Position.Y);

            if (Destination != null)
            {
                Console.WriteLine("Destination: " + Destination.X + " " + Destination.Y);
            }
        }

        public void Move(Location destination)
        {
            _status = "Moving";
            IsMoving = true;

            Destination = destination;

            _movementTimer.Interval = MovementSpeed;
            _movementTimer.Start();
        }

        private void _movementTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Destination.X > Position.X)
            {
                Position.X++;
            }

            if (Destination.X < Position.X)
            {
                Position.X--;
            }

            if (Destination.Y > Position.Y)
            {
                Position.Y++;
            }

            if (Destination.Y < Position.Y)
            {
                Position.Y--;
            }

            if (Destination.X == Position.X && Destination.Y == Position.Y)
            {
                IsMoving = false;
                Position = Destination;
                Destination = null;

                _status = "Idle";

                _movementTimer.Stop();
            }
        }
    }
}
