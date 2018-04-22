using System;
using System.Collections.Generic;

namespace App.Schedule.Domains.Helpers
{
    public class Hour
    {
        public static Dictionary<int, string> GetHoursOfDay()
        {
            var hours = new Dictionary<int, string>();
            try
            {
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0);
                for (var i = 1; i <= 48; i++)
                {
                    hours.Add(i, date.ToString("hh:mm tt"));
                    date = date.AddMinutes(30);
                }
            }
            catch {
            }
            return hours;
        }
    }
}
