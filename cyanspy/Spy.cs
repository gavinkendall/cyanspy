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

        public Location Source { get; set; }
        public Location Destination { get; set; }

        public DateTime TravelStartTime { get; set; }
        public DateTime TravelEndTime { get; set; }

        public double HP { get; set; }

        public SpyAction Action { get; set; }
        public SpyCondition Condition { get; set; }

        public Spy()
        {
            Action = new SpyAction();
            Condition = new SpyCondition();

            Id = GenerateRandomNumber(MIN_ID, MAX_ID);
            HP = GenerateRandomNumber((int)MIN_HEALTH, (int)MAX_HEALTH);

            if (HP >= MAX_HEALTH || HP > CRITICAL_HEALTH)
            {
                Condition.Healthy = true;
            }

            if (HP > 0 && HP < CRITICAL_HEALTH)
            {
                Condition.Healthy = false;
            }

            if (HP <= 0)
            {
                Condition.Deceased = true;
            }
        }

        private int GenerateRandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
