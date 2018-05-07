using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using System.Threading.Tasks;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class DashboardController : DashboardBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (this.LogoutStatus())
                return RedirectToAction("Login", "Home", new { area = "Admin" });
            else
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [NonAction]
        private async Task<List<AppointmentViewModel>> GetAppointments()
        {
            var data = new List<AppointmentViewModel>();
            var response = await this.AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response.Status)
            {
                data = response.Data;
            }
            return data;
        }

        //[NonAction]
        //public List<AppointmentViewModel> SampleTestData(DateTime start, DateTime end)
        //{
        //    return new List<AppointmentViewModel>()
        //    {
        //        new AppointmentViewModel()
        //        {
        //            Id = 1,
        //            Title = "Test Appointment 1",
        //            StartTime = DateTime.Now.AddHours(10),
        //            EndTime = DateTime.Now.AddHours(12),
        //            BackColor = 123456,
        //            TextColor = 121212,
        //            IsAllDayEvent = true
        //        },
        //        new AppointmentViewModel()
        //        {
        //            Id = 2,
        //            Title = "Test Appointment 2",
        //            StartTime = DateTime.Now.AddDays(4).AddHours(10),
        //            EndTime = DateTime.Now.AddDays(4).AddHours(12),
        //            BackColor = 123456,
        //            TextColor = 121212,
        //            IsAllDayEvent = false
        //        }
        //    };
        //}

        [HttpGet]
        public async Task<JsonResult> GetDiaryEvents(DateTime start, DateTime end)
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
                allDay = x.IsAllDayEvent
            }).ToArray();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}