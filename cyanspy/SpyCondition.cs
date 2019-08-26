namespace cyanspy
{
    public class SpyCondition
    {
        public bool Drunk { get; set; }
        public bool Healthy { get; set; }
        public bool Tired { get; set; }
        public bool Poisoned { get; set; }
        public bool Deceased { get; set; }

        public string Status
        {
            get
            {
                if (Drunk)
                {
                    return "Drunk";
                }

                

                if (Poisoned)
                {
                    return "Poisoned";
                }

                if (Deceased)
                {
                    return "Deceased";
                }

                if (Healthy)
                {
                    return "Healthy";
                }
                else
                {
                    return "Critical";
                }
            }
        }

        public SpyCondition()
        {
            
        }
    }
}
