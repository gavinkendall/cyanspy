using System;
using System.Collections.Generic;

namespace cyanspy
{
    public class Mech
    {
        // A mech has applications, components, and weapons.
        public Dictionary<string, Application> Applications { get; set; }
        public Dictionary<string, Component> Components { get; set; }
        public Dictionary<string, Weapon> Weapons { get; set; }

        public string Name { get; set; }
        public string Mnemonic { get; private set; }
        public Location Source { get; set; }
        public Location Destination { get; set; }
        public TimeSpan TimeToDestination { get; set; }
        public DateTime EstimatedTimeOfArrival { get; set; }
        public bool IsMoving { get; set; }

        public Mech(string name, string mnemonic)
        {
            Name = name;
            Mnemonic = mnemonic;

            Source = new Location(Name, Mnemonic);
            Destination = new Location(Name, Mnemonic);
            Destination = Source;

            IsMoving = false;
        }

        public void Move(Location destination, TimeSpan timeToDestination)
        {
            Destination = destination;
            TimeToDestination = timeToDestination;
            IsMoving = true;
            EstimatedTimeOfArrival = DateTime.Now + TimeToDestination;
        }

        public bool DestinationReached()
        {
            if (IsMoving)
            {
                if (DateTime.Now >= EstimatedTimeOfArrival)
                {
                    TimeToDestination = new TimeSpan();
                    EstimatedTimeOfArrival = DateTime.Now;
                    IsMoving = false;

                    return true;
                }
            }

            return false;
        }
    }
}
