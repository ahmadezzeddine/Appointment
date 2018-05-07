using App.Schedule.Domains.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<JsonResult> GetDiaryEvents(DateTime start, DateTime end)
        {
            var data = (await GetAppointments()).Select(x => new
            {
                id = x.Id,
                title = x.Title + " Customer: " + x.BusinessCustomerName + " Service Name: "+ x.BusinessServiceName + " Offer: " + x.BusinessOfferName,
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