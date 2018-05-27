using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Employee.Controllers.Base;
using FullCalendar;

namespace App.Schedule.Web.Areas.Employee.Controllers
{
    public class CalendarController : CalendarBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.BusinessHours = await this.GetBusinessHours();
            return View();
        }
        public async Task<ActionResult> Week()
        {
            ViewBag.BusinessHours = await this.GetBusinessHours();
            return View();
        }
        public async Task<ActionResult> Timeline()
        {
            ViewBag.BusinessHours = await this.GetBusinessHours();
            return View();
        }
        
        [HttpGet]
        public async Task<IEnumerable<BusinessHour>> GetBusinessHours()
        {
            var businessHours = new List<BusinessHour>();
            try
            {
                var response = await this.BusinessHourService.Gets(RegisterViewModel.Employee.ServiceLocationId.Value, TableType.ServiceLocationId);
                if (response != null && response.Status)
                {
                    foreach (var hour in response.Data)
                    {
                        if (!hour.IsHoliday)
                        {
                            var dow = new List<DayOfWeek>();
                            dow.Add((DayOfWeek)hour.WeekDayId);
                            var businessHour = new BusinessHour()
                            {
                                Dow = dow,
                                Start = new TimeSpan(hour.From.Ticks),
                                End = new TimeSpan(hour.To.Ticks)
                            };
                            businessHours.Add(businessHour);
                            if (hour.IsSplit1 != null && hour.IsSplit1.Value)
                            {
                                businessHour = new BusinessHour()
                                {
                                    Dow = dow,
                                    Start = new TimeSpan(hour.FromSplit1.Value.Ticks),
                                    End = new TimeSpan(hour.ToSplit1.Value.Ticks)
                                };
                                businessHours.Add(businessHour);
                            }
                            if (hour.IsSplit2 != null && hour.IsSplit2.Value)
                            {
                                businessHour = new BusinessHour()
                                {
                                    Dow = dow,
                                    Start = new TimeSpan(hour.FromSplit2.Value.Ticks),
                                    End = new TimeSpan(hour.ToSplit2.Value.Ticks)
                                };
                                businessHours.Add(businessHour);
                            }
                        }
                    }
                }
                return businessHours;
            }
            catch
            {
                return businessHours;
            }
        }
    }
}