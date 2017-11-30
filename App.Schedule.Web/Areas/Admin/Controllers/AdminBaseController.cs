using System;
using System.Web;
using System.Web.Mvc;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        protected string Token;
        protected HttpCookie AdminCookie;
        protected BusinessEmployeeViewModel BusinessEmployee;
        protected CountryService CountryService;
        protected BusinessCategoryService BusinessCategoryService;
        protected TimezoneService TimezoneService;
        protected MembershipService MembershipService;
        protected BusinessService BusinessService;
        protected BusinessEmployeeService BusinessEmployeeService;

        public AdminBaseController()
        {
            BusinessEmployee = new BusinessEmployeeViewModel();
        }

        [NonAction]
        public bool SetAdminSession(BusinessEmployeeViewModel model, bool isKeepLoggedIn, string token)
        {
            try
            {
                Session["aEmail"] = model.Email;
                var businessEmployee = new HttpCookie("aadminappointment");

                if (isKeepLoggedIn)
                    businessEmployee.Expires = DateTime.Now.AddDays(1);
                else
                    businessEmployee.Expires = DateTime.Now.AddDays(365);

                businessEmployee.Values["aFirstName"] = model.FirstName;
                businessEmployee.Values["aLastName"] = model.LastName;
                businessEmployee.Values["aEmail"] = model.Email;
                businessEmployee.Values["aPassword"] = model.Password;
                businessEmployee.Values["aIsAdmin"] = model.IsAdmin ? "true" : "false";
                businessEmployee.Values["aIsActive"] = model.IsActive ? "true" : "false";
                businessEmployee.Values["aToken"] = token;
                businessEmployee.Values["aId"] = Convert.ToString(model.Id);
                Response.Cookies.Add(businessEmployee);

                return true;
            }
            catch
            {
                return false;
            }
        }

        [NonAction]
        public BusinessEmployeeViewModel GetAdminSession()
        {
            try
            {
                BusinessEmployee = new BusinessEmployeeViewModel();
                if (Request.Cookies["aadminappointment"] != null)
                {
                    AdminCookie = HttpContext.Request.Cookies["aadminappointment"];
                    if (AdminCookie != null)
                    {
                        BusinessEmployee.FirstName = AdminCookie.Values["aFirstName"];
                        BusinessEmployee.LastName = AdminCookie.Values["aLastName"];
                        BusinessEmployee.Email = AdminCookie.Values["aEmail"];
                        BusinessEmployee.IsActive = Convert.ToBoolean(AdminCookie.Values["aIsActive"]);
                        BusinessEmployee.IsAdmin = Convert.ToBoolean(AdminCookie.Values["aIsAdmin"]);
                        Token = AdminCookie.Values["aToken"];
                        BusinessEmployee.Id = Convert.ToInt64(AdminCookie.Values["aId"]);
                        return BusinessEmployee;
                    }
                    else
                        return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

        [NonAction]
        protected bool LoginStatus()
        {
            try
            {
                BusinessEmployee = GetAdminSession();
                this.CountryService = new CountryService(Token);
                this.BusinessCategoryService = new BusinessCategoryService(Token);
                this.TimezoneService = new TimezoneService(Token);
                this.MembershipService = new MembershipService(Token);
                this.BusinessService = new BusinessService(Token);
                this.BusinessEmployeeService = new BusinessEmployeeService(Token);
                //Call service;
                if (BusinessEmployee != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!LoginStatus())
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Admin"});
        }
    }
}