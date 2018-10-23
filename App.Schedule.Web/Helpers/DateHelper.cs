using System;

namespace App.Schedule.Web.Helpers
{
    public static class DateHelper
    {
        public static DateTime UtcToLocal(this DateTime date)
        {
            if (date != null)
                return date.ToLocalTime();
            else return date;
        }
    }
}