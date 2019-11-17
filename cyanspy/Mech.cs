using System;
using System.Collections.Generic;

namespace cyanspy
{
    public class Mech
    {
        // A mech has pilots, applications, components, and weapons.
        public Dictionary<string, Pilot> Pilots {get; set; }

        public string Name { get; set; }
        public string Mnemonic { get; private set; }

        public Mech(string name)
        {
            Name = name;
            Mnemonic = "M";
            Pilots = new Dictionary<string, Pilot>();
        }
    }
}
