using System;
using System.Web;
using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Customer.Controllers.Base
{
    public class DashboardBaseController : BaseController
    {
        protected AppointmentService AppointmentService;
        protected BusinessService BusinessService;
        protected BusinessHourService BusinessHourService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("Index", "Dashboard", new { area = "Customer" });
            }
            else
            {
                this.AppointmentService = new AppointmentService(this.Token);
                this.BusinessService = new BusinessService(this.Token);
                this.BusinessHourService = new BusinessHourService(this.Token);
            }
        }

        [NonAction]
        public bool LogoutStatus()
        {
            try
            {
                if (Request.Cookies[httpCookieKey] != null)
                {
                    var admin = new HttpCookie(httpCookieKey);
                    if (Session.Keys.Count > 0) { Session["aEmail"] = ""; }
                    admin.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(admin);
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}