﻿using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class CalendarBaseController : BaseController
    {
        protected ServiceLocationService ServiceLocationService;
        protected AppointmentService AppointmentService;
        protected BusinessHourService BusinessHourService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("login", "home", new { area = "admin" });
            }
            else
            {
                this.ServiceLocationService = new ServiceLocationService(this.Token);
                this.AppointmentService = new AppointmentService(this.Token);
                this.BusinessHourService = new BusinessHourService(this.Token);
            }
        }
    }
}