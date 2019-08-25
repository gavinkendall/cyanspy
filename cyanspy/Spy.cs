using System;

namespace cyanspy
{
    public class Spy
    {
        private const double MIN_HEALTH = 16.3;
        private const double MAX_HEALTH = 100;
        private const double CRITICAL_HEALTH = 20.1;

        private const int MIN_ID = 1;
        private const int MAX_ID = 99;

        public readonly int HiringCost = Cost.HiringCostPerSpy;

        public int Id { get; set; }
        public string Name { get; set; }
        public bool Deployed { get; set; }

        public Location Location { get; set; }
        public string Destination { get; set; }
        public DateTime TravelStartTime { get; set; }
        public DateTime TravelEndTime { get; set; }

        public double HP { get; set; }
        public bool Drunk { get; set; }
        public bool Healthy { get; set; }
        public bool Poisonded { get; set; }
        public string Condition { get; set; }

        public Spy()
        {
            Name = "Anonymous";

            Deployed = false;

            Id = GenerateRandomNumber(MIN_ID, MAX_ID);
            HP = GenerateRandomNumber((int)MIN_HEALTH, (int)MAX_HEALTH);

            if (HP >= MAX_HEALTH || HP > CRITICAL_HEALTH)
            {
                Healthy = true;
                Condition = "Healthy";
            }

            if (HP > 0 && HP < CRITICAL_HEALTH)
            {
                Condition = "Critical";
            }

            if (HP <= 0)
            {
                Condition = "Deceased";
            }
        }

        private int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
