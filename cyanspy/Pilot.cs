using System;

namespace cyanspy
{
    public class Pilot
    {
        public string Name { get; private set; }

        public Pilot(string name)
        {
            Name = name;
        }
    }
}
