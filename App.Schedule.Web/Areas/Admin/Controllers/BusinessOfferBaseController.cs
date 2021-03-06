﻿using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class BusinessOfferBaseController : BaseController
    {
        protected BusinessOfferService BusinessOfferService;
        protected BusinessOfferServiceLocationService BusinessOfferServiceLocationService;
        protected ServiceLocationService ServiceLocationService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Admin" });
            }
            else
            {
                this.BusinessOfferService = new BusinessOfferService(this.Token);
                this.BusinessOfferServiceLocationService = new BusinessOfferServiceLocationService(this.Token);
                this.ServiceLocationService = new ServiceLocationService(this.Token);
            }
        }
    }
}