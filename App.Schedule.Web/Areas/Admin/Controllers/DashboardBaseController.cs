using App.Schedule.Web.Services;
using System;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class DashboardBaseController : BaseController
    {
        protected AppointmentService AppointmentService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Admin" });
            }
            else
            {
                this.AppointmentService = new AppointmentService(this.Token);
            }
        }

        [NonAction]
        public bool LogoutStatus()
        {
            try
            {
                if (Request.Cookies["aadminappointment"] != null)
                {
                    var admin = new HttpCookie("aadminappointment");
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