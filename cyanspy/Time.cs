using System;

namespace cyanspy
{
    public static class Time
    {
        public static bool Enabled { get; set; }

        public static string Show()
        {
            if (Enabled)
            {
                return DateTime.Now.ToString("hh:mm:ss") + " ";
            }

            return string.Empty;
        }
    }
}
