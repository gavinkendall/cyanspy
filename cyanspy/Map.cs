﻿using System;
using System.Collections.Generic;

namespace cyanspy
{
    public class Map
    {
        private Dictionary<string, Location> _map;

        public bool Enabled { get; set; }

        public Map()
        {
            _map = new Dictionary<string, Location>();
        }

        private string DisplayLocation(int x, int y)
        {
            foreach (Location location in _map.Values)
            {
                if (location.X == x && location.Y == y)
                {
                    return location.Mnemonic;
                }
            }

            return ".";
        }

        /// <summary>
        /// Adds a new location to the map.
        /// </summary>
        /// <param name="newLocation">Location to add to map</param>
        public void AddLocation(Location newLocation)
        {
            // Recursively keep adding the new location until it no longer
            // has X and Y coordinates that conflict with X and Y coordinates
            // of any existing location on the map.
            foreach(Location existingLocation in _map.Values)
            {
                if ((newLocation.X == existingLocation.X) &&
                    (newLocation.Y == existingLocation.Y))
                {
                    AddLocation(new Location(newLocation.Name, newLocation.Mnemonic));
                }
            }

            if (!_map.ContainsKey(newLocation.Name))
            {
                _map.Add(newLocation.Name, newLocation);
            }
        }

        public void RemoveLocation(Location location)
        {
            if (_map.ContainsKey(location.Name))
            {
                _map.Remove(location.Name);
            }
        }

        /// <summary>
        /// Gets a location by its name.
        /// </summary>
        /// <param name="name">Name of location to find</param>
        /// <returns>Location found by its name</returns>
        public Location GetLocationByName(string name)
        {
            return _map[name];
        }

        /// <summary>
        /// This will render a 10x10 map of the battlefield.
        /// </summary>
        public void Render()
        {
            if (!Enabled) return;

            /*
                  0  1  2  3  4  5  6  7  8  9  
                9 .  .  .  .  .  .  .  .  .  . 9
                8 .  .  .  .  .  .  .  .  .  . 8
                7 .  .  .  .  .  .  .  .  .  . 7
                6 .  .  .  .  .  .  .  .  .  . 6
                5 .  .  .  .  .  .  .  .  .  . 5
                4 .  .  .  .  .  .  .  .  .  . 4
                3 .  .  .  .  .  .  .  .  .  . 3
                2 .  .  .  .  .  .  .  .  .  . 2
                1 .  .  .  .  .  .  .  .  .  . 1
                0 .  .  .  .  .  .  .  .  .  . 0
                  0  1  2  3  4  5  6  7  8  9  
                */

            for (int y = 10; y >= -1; y--)
            {
                for (int x = -1; x <= 10; x++)
                {
                    // Display whitespace if we're on the North West corner or the South West corner of the map to pad it out a little.
                    // (We don't care about the North East or South East corners because they would already have whitespace included)
                    if ((x == -1 && y == 10) || (x == -1 && y == -1))
                    {
                        Console.Write("  ");
                    }

                    // For anything in between the top edge and the bottom edge of the map and at the left edge or the right edge of the map.
                    if ((y < 10 && y > -1) && (x == -1 || x == 10))
                    {
                        // Display the Y coordinate value if we're either at the left edge (x == -1) or the right edge (x == 10) of the map
                        // and we're somewhere in between the top edge and the bottom edge of the map (y < 10 && y > -1).
                        Console.Write(y.ToString());
                    }

                    // For anything in between the left edge and the right edge of the map.
                    if (x > -1 && x < 10)
                    {
                        if (y == 10 || y == -1)
                        {
                            // Display the X coordinate value if we're either at the top edge (y == 10) or at the bottom edge (y == -1) of the map
                            // and we're somewhere in between the left edge and the right edge of the map (x > -1 && x < 10).
                            Console.Write(x.ToString() + "  ");
                        }
                        else
                        {
                            // Otherwise, display a location on the map.
                            Console.Write(" " + DisplayLocation(x, y) + " ");
                        }
                    }

                    if (x == 10)
                    {
                        // Display a new line if we've reached the right edge of the map.
                        Console.WriteLine();
                    }
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Gets the distance between two locations. Uses math.
        /// </summary>
        /// <param name="source">Source location</param>
        /// <param name="target">Target location</param>
        /// <returns>Distance between source and target</returns>
        public double GetDistanceBetweenLocations(Location source, Location target)
        {
            int dx = source.X - target.X;
            int dy = source.Y - target.Y;

            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Determines if a source is within a specified range of a target.
        /// </summary>
        /// <param name="source">Source location</param>
        /// <param name="target">Target location</param>
        /// <param name="range">Range.
        /// Bigger range is more likely for locations to be within range.
        /// Smaller range is less likely for locations to be within range.</param>
        /// <returns>True if locations are within range. False if locations are not within range</returns>
        public bool AreLocationsWithinRange(Location source, Location target, int range)
        {
            return GetDistanceBetweenLocations(source, target) <= range;
        }

        /// <summary>
        /// Gets the duration between two locations.
        /// </summary>
        /// <param name="source">Source location</param>
        /// <param name="target">Target location</param>
        /// <returns>Duration between two locations</returns>
        public TimeSpan GetTimeSpanBetweenLocations(Location source, Location target)
        {
            int hours = 0;
            int minutes = (int)GetDistanceBetweenLocations(source, target);
            int seconds = 0;

            return new TimeSpan(hours, minutes, seconds);
        }
    }
}
