using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Web.Admin.Models;

namespace App.Schedule.Web.Admin.Controllers
{
    public class DashboardController : DashboardBaseController
    {
        public ActionResult Logout()
        {
            if (Request.Cookies["aappointment"] != null)
            {
                var admin = new HttpCookie("aappointment");
                Session["aEmail"] = "";
                admin.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(admin);
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<ActionResult> Index()
        {
            var model = new DashboardViewModel();
            try
            {
                Session["HomeLink"] = "Dashboard";
                if (admin != null)
                {
                    var admins = await this.DashboardService.GetAdmins();
                    var countries = await this.DashboardService.GetCountries();
                    var timezones = await this.DashboardService.GetTimezones();
                    var memberships = await this.DashboardService.GetMemberships();
                    var businessCategories = await this.DashboardService.GetBusinessCategories();
                    var businesses = await this.DashboardService.GetBusinesses();
                    model.AdminsCount = (admin != null) ? admins.Count() : 0;
                    model.CountryCount = (countries != null) ? countries.Count() : 0;
                    model.TimezonCount = (timezones != null) ? timezones.Count() : 0;
                    model.MembershipCount = (memberships != null) ? memberships.Count() : 0;
                    model.BusinessCategoryCount = (businessCategories != null) ? businessCategories.Count() : 0;
                    model.BusinessCount = (businesses != null) ? businesses.Count() : 0;
                }
                else
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }
    }
}