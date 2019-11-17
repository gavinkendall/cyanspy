using System;
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

        public void Add(Location location)
        {
            _map.Add(location.Name, location);
        }

        public Location GetLocationByName(string name)
        {
            return _map[name];
        }

        /// <summary>
        /// This will render a 10x10 map of the battlefield.
        /// </summary>
        public void Render()
        {
            /*
              0  1  2  3  4  5  6  7  8  9  
            0 .  .  .  .  .  .  .  .  .  . 0
            1 .  .  .  .  .  .  .  .  .  . 1
            2 .  .  .  .  .  .  .  .  .  . 2
            3 .  .  .  .  .  .  .  .  .  . 3
            4 .  .  .  .  .  .  .  .  .  . 4
            5 .  .  .  .  .  .  .  .  .  . 5
            6 .  .  .  .  .  .  .  .  .  . 6
            7 .  .  .  .  .  .  .  .  .  . 7
            8 .  .  .  .  .  .  .  .  .  . 8
            9 .  .  .  .  .  .  .  .  .  . 9
              0  1  2  3  4  5  6  7  8  9  
            */

            for (int y = -1; y <= 10; y++)
            {
                for (int x = -1; x <= 10; x++)
                {
                    // Display whitespace if we're on the North West corner or the South West corner of the map to pad it out a little.
                    // (We don't care about the North East or South East corners because they would already have whitespace included)
                    if ((x == -1 && y == -1) || (x == -1 && y == 10))
                    {
                        Console.Write("  ");
                    }

                    // For anything in between the top edge and the bottom edge of the map and at the left edge or the right edge of the map.
                    if ((y > -1 && y < 10) && (x == -1 || x == 10))
                    {
                        // Display the Y coordinate value if we're either at the left edge (x == -1) or the right edge (x == 10) of the map
                        // and we're somewhere in between the top edge and the bottom edge of the map (y > -1 && y < 10).
                        Console.Write(y.ToString());
                    }

                    // For anything in between the left edge and the right edge of the map.
                    if (x > -1 && x < 10)
                    {
                        if (y == -1 || y == 10)
                        {
                            // Display the X coordinate value if we're either at the top edge (y == -1) or at the bottom edge (y == 10) of the map
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

        private string DisplayLocation(int x, int y)
        {
            foreach(Location location in _map.Values)
            {
                if (location.X == x && location.Y == y)
                {
                    return location.Mnemonic;
                }
            }

            return ".";
        }
    }
}
