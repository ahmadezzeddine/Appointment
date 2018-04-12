using App.Schedule.Domains.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class CalendarController : CalendarBaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Week()
        {
            return View();
        }

        public ActionResult Timeline()
        {
            return View();
        }

        public ActionResult Reports()
        {
            return View();
        }

        [NonAction]
        public List<AppointmentViewModel> SampleTestData(DateTime start, DateTime end)
        {
            return new List<AppointmentViewModel>()
            {
                new AppointmentViewModel()
                {
                    Id = 1,
                    Title = "Test Appointment 1",
                    StartTime = DateTime.Now.AddHours(10),
                    EndTime = DateTime.Now.AddHours(12),
                    BackColor = 123456,
                    TextColor = 121212,
                    IsAllDayEvent = true
                },
                new AppointmentViewModel()
                {
                    Id = 2,
                    Title = "Test Appointment 2",
                    StartTime = DateTime.Now.AddDays(4).AddHours(10),
                    EndTime = DateTime.Now.AddDays(4).AddHours(12),
                    BackColor = 123456,
                    TextColor = 121212,
                    IsAllDayEvent = false
                }
            };
        }

        [HttpGet]
        public JsonResult GetDiaryEvents(DateTime start, DateTime end)
        {
            var result = SampleTestData(start, end).Select(x => new
            {
                id = x.Id,
                title = x.Title,
                start = x.StartTime,
                end = x.EndAfter,
                color = x.BackColor,
                className = "",
                someKey = x.Id,
                allDay = x.IsAllDayEvent
            }).ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}