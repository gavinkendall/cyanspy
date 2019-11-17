using System;

namespace cyanspy
{
    public class Location
    {
        private const int MIN = 0;
        private const int MAX = 9;

        public string Name { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Location(string name)
        {
            Name = name;
            X = GenerateRandomNumber(MIN, MAX);
            Y = GenerateRandomNumber(MIN, MAX);
        }

        private int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
