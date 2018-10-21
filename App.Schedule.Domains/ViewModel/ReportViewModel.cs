using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Schedule.Domains.ViewModel
{
    public enum ReportType
    {
        Custom = 0,
        Today = 1,
        Tomorrow = 2,
        Yesterday = 3,
        ThisWeek = 4,
        NextWeek = 5,
        LastWeek = 6,
        ThisMonth = 7,
        NextMonth = 8,
        LastMonth = 9,
        ThisYear = 10,
        NextYear = 11,
        LastYear = 12
    }

    public class ReportViewModel
    {
        public ReportType ReportTypeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public CheckSelectionBox hasOnlyBusinessEmployeeId { get; set; }
        public List<CheckSelectionBox> CustomerId { get; set; }
        public List<CheckSelectionBox> AppointmentStatusTypeId { get; set; }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            return new DateTime(dt.Year, 1, 1);
        }
    }
}
