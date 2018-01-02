﻿using System.Web.Mvc;
using App.Schedule.Services;

namespace App.Schedule.Web.Admin.Controllers
{
    public class AdminBaseController : BaseController
    {

        protected AdminService AdminService;
        protected DashboardService DashboardService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = this.LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Admin" });
            }
            else
            {
                this.AdminService = new AdminService(this.Token);
                this.DashboardService = new DashboardService(this.Token);
            }
        }
    }
}