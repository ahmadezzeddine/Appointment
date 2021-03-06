﻿using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class HomeBaseController : BaseController
    {
        protected BusinessEmployeeService BusinessEmployeeService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (status)
            {
                filterContext.Result = RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                this.BusinessEmployeeService = new BusinessEmployeeService(this.Token);
            }
        }
    }
}