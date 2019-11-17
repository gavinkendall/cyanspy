using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace cyanspy
{
    public class Program
    {
        private static Map _map;
        private static Time _time;

        public static void Main()
        {
            _map = new Map();
            _time = new Time();

            string commandInput = string.Empty;

            do
            {
                if (_map.Enabled)
                {
                    _map.Render();
                }

                Console.Write(_time.Show() + "> ");
                commandInput = Console.ReadLine();

                Regex rgxCommandInput = new Regex(@"(?<Command>[a-z]+) ?(?<Value1>[0-9a-zA-Z]+)? ?(?<Value2>[0-9a-zA-Z]+)?");

                string command = rgxCommandInput.Match(commandInput).Groups["Command"].Value;
                string value1 = rgxCommandInput.Match(commandInput).Groups["Value1"].Value;
                string value2 = rgxCommandInput.Match(commandInput).Groups["Value2"].Value;

                switch (command.ToLower())
                {
                    case Command.Move:
                        break;

                    case Command.Map:
                        if (value1.Equals("on"))
                        {
                            _map.Enabled = true;
                        }

                        if (value1.Equals("off"))
                        {
                            _map.Enabled = false;
                        }
                        break;

                    case Command.Time:
                        if (value1.Equals("on"))
                        {
                            _time.Enabled = true;
                        }

                        if (value1.Equals("off"))
                        {
                            _time.Enabled = false;
                        }
                        break;
                }

                Console.Clear();
            }
            while (!commandInput.Equals(Command.Exit));
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