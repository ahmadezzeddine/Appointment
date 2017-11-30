using System;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class DashboardController : AdminBaseController
    {
        [HttpGet]
        public ActionResult Logout()
        {
            if (Request.Cookies["aadminappointment"] != null)
            {
                var admin = new HttpCookie("aadminappointment");
                Session["aEmail"] = "";
                admin.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(admin);
                return RedirectToAction("Login", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Login", "Home", new { area = "Admin" });
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}