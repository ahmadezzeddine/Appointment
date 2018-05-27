using System;
using System.Web;
using FullCalendar;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class CalendarController : CalendarBaseController
    {
        public async Task<ActionResult> Index()
        {
            ViewBag.BusinessHours =await this.GetBusinessHours();
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

        public ActionResult Reports()
        {
            return View();
        }

        [NonAction]
        private async Task<List<AppointmentViewModel>> GetAppointments()
        {
            var data = new List<AppointmentViewModel>();
            var response = await this.AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response.Status)
            {
                if (response.Data != null)
                {
                    response.Data.ForEach(appointment =>
                    {
                        appointment.Created = appointment.Created.ToLocalTime();
                        appointment.StartDate = appointment.StartDate.Value.ToLocalTime();
                        appointment.StartTime = appointment.StartTime.Value.ToLocalTime();
                        appointment.EndDate = appointment.EndDate.Value.ToLocalTime();
                        appointment.EndTime = appointment.EndTime.Value.ToLocalTime();
                    });
                }
                data = response.Data;
            }
            return data;
        }

        [HttpGet]
        public async Task<JsonResult> GetDiaryEvents()
        {
            var data = (await GetAppointments()).Select(x => new
            {
                id = x.Id,
                title = x.Title + " Customer: " + x.BusinessCustomerName + " Service Name: " + x.BusinessServiceName + " Offer: " + x.BusinessOfferName,
                start = x.StartTime,
                end = x.EndTime,
                color = x.BackColor,
                className = "",
                someKey = x.Id,
                allDay = x.IsAllDayEvent,
                dow = new int[1, 4]
            }).ToArray();
            return Json(data, JsonRequestBehavior.AllowGet);
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