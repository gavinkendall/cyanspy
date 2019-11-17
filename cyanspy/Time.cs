using System;

namespace cyanspy
{
    public class Time
    {
        public bool Enabled { get; set; }

        public Time()
        {

        }

        public string Show()
        {
            if (Enabled)
            {
                return DateTime.Now.ToString("hh:mm:ss") + " ";
            }

            return string.Empty;
        }
    }
}
