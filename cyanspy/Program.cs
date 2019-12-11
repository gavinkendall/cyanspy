using System;

namespace cyanspy
{
    public class Program
    {
        public static void Main()
        {
            try
            {
                Map map = new Map();

                Mech mechAtomicRabbit = new Mech("Atomic Rabbit", "R");
                Mech mechCosmicWolf = new Mech("Cosmic Wolf", "W");

                map.AddLocation(mechAtomicRabbit.Position);
                map.AddLocation(mechCosmicWolf.Position);

                // Setup just for development/debugging.
                Time.Enabled = true;
                map.Enabled = true;
                mechAtomicRabbit.ShowInfo = true;

                mechAtomicRabbit.InitiateCommandInterface(map);

                //Console.WriteLine("Mech Name = " + mechAtomicRabbit.Name);
                //Console.WriteLine("Mech Mnemonic = " + mechAtomicRabbit.Mnemonic);
                //Console.WriteLine("Mech Moving = " + mechAtomicRabbit.IsMoving.ToString());
                //Console.WriteLine("Mech Position = " + mechAtomicRabbit.Position.X + " " + mechAtomicRabbit.Position.Y);

                //if (mechAtomicRabbit.IsMoving)
                //{
                //    Console.WriteLine("Mech Destination = " + mechAtomicRabbit.Destination.X + " " + mechAtomicRabbit.Destination.Y);
                //}

                //if (map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 1))
                //{
                //    Console.WriteLine("Mech within melee combat range of enemy");
                //}

                //if(map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 2))
                //{
                //    Console.WriteLine("Mech within machine gun range of enemy");
                //}

                //if (map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 3))
                //{
                //    Console.WriteLine("Mech within missile launch range of enemy");
                //}

                //if (map.AreLocationsWithinRange(mechAtomicRabbit.Position, mechCosmicWolf.Position, range: 4))
                //{
                //    Console.WriteLine("Mech within radar scan range of enemy");
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}