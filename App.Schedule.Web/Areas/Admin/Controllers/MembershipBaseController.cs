﻿using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class MembershipBaseController : BaseController
    {
        protected MembershipService MembershipService;
        protected RegisterService BusinessService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = this.LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Admin" });
            }
            else
            {
                this.MembershipService = new MembershipService(this.Token);
                this.BusinessService = new RegisterService(this.Token);
            }
        }
    }
}