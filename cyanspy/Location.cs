using System;

namespace cyanspy
{
    public class Location
    {
        public string Name { get; }
        public TimeSpan FlightDuration { get; }

        public Location(string name, TimeSpan flightDuration)
        {
            Name = name;
            FlightDuration = flightDuration;
        }
    }
}
