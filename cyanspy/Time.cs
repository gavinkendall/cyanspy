using System;

namespace cyanspy
{
    public static class Time
    {
        public static bool Enabled { get; set; }

        /// <summary>
        /// Shows the current time.
        /// </summary>
        /// <returns>A formatted timestamp as a string</returns>
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
