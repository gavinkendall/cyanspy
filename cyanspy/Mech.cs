using System;
using System.Timers;
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
        public Location Destination { get; set; }
        public Location Position { get; set; }
        public bool IsMoving { get; set; }
        public int MovementSpeed { get; set; }

        private Timer _movementTimer;

        public Mech(string name, string mnemonic)
        {
            Name = name;
            Mnemonic = mnemonic;

            Position = new Location(Name, Mnemonic);

            IsMoving = false;
            MovementSpeed = 10000;
            _movementTimer = new Timer();
            _movementTimer.Elapsed += _movementTimer_Elapsed;
        }

        public void Move(Location destination)
        {
            Destination = destination;

            _movementTimer.Interval = MovementSpeed;
            _movementTimer.Start();

            _movementTimer_Elapsed(null, null);
        }

        private void _movementTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsMoving = true;

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
                _movementTimer.Stop();
            }
        }
    }
}
