using System;

namespace App.Schedule.Web.Models
{
    public class SchedulerModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string color { get; set; }
        public string textColor { get; set; }
        public string className { get; set; }
        public long someKey { get; set; }
        public bool allDay { get; set; }
        public string url { get; set; }
        public string description { get; set; }
    }
}