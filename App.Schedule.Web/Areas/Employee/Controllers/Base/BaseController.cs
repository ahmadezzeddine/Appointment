using System;
using System.Web;
using System.Web.Mvc;
using App.Schedule.Web.Helpers;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Employee.Controllers.Base
{
    public class BaseController : Controller
    {
        protected string Token;
        protected HttpCookie AdminCookie;
        protected RegisterViewModel RegisterViewModel;
        protected ResponseHelper ResponseHelper;
        public const string httpCookieKey = "aemployeeappointment";

        public BaseController()
        {
            this.RegisterViewModel = new RegisterViewModel();
            this.ResponseHelper = new ResponseHelper();
        }

        [NonAction]
        public bool SetAdminSession(RegisterViewModel model, bool isKeepLoggedIn, string token)
        {
            try
            {
                Session["aEmail"] = model.Employee.Email;
                var businessEmployee = new HttpCookie(httpCookieKey);

                //if (isKeepLoggedIn)
                //    businessEmployee.Expires = DateTime.Now.AddDays(365);
                //else
                    //businessEmployee.Expires = DateTime.Now.AddHours(1);

                businessEmployee.Values["aFirstName"] = model.Employee.FirstName;
                businessEmployee.Values["aLastName"] = model.Employee.LastName;
                businessEmployee.Values["aEmail"] = model.Employee.Email;
                businessEmployee.Values["aPassword"] = model.Employee.Password;
                businessEmployee.Values["aIsAdmin"] = model.Employee.IsAdmin ? "true" : "false";
                businessEmployee.Values["aIsActive"] = model.Employee.IsActive ? "true" : "false";
                businessEmployee.Values["aToken"] = token;
                businessEmployee.Values["aEmpId"] = Convert.ToString(model.Employee.Id);
                businessEmployee.Values["aBusinessId"] = Convert.ToString(model.Business.Id);
                businessEmployee.Values["aMembershipId"] = Convert.ToString(model.Business.MembershipId);
                businessEmployee.Values["aBusinessCategoryId"] = Convert.ToString(model.Business.BusinessCategoryId);
                businessEmployee.Values["aTimezoneId"] = Convert.ToString(model.Business.TimezoneId);
                businessEmployee.Values["aServiceLocationId"] = Convert.ToString(model.Employee.ServiceLocationId);
                businessEmployee.Values["aBusinessName"] = string.Format("{0} ({1})", model.Business.Name, model.Business.ShortName);
                businessEmployee.Values["aServiceLocationName"] = model.ServiceLocation.Name;
                businessEmployee.Values["aTotalAppointment"] = Convert.ToString(model.Business.tblMembership.TotalAppointment);
                businessEmployee.Values["aTotalCustomer"] = Convert.ToString(model.Business.tblMembership.TotalCustomer);
                businessEmployee.Values["aTotalEmployee"] = Convert.ToString(model.Business.tblMembership.TotalEmployee);
                businessEmployee.Values["aTotalLocation"] = Convert.ToString(model.Business.tblMembership.TotalLocation);
                businessEmployee.Values["aTotalOffers"] = Convert.ToString(model.Business.tblMembership.TotalOffers);
                businessEmployee.Values["aIsUnlimited"] = Convert.ToString(model.Business.tblMembership.IsUnlimited);

                Response.Cookies.Add(businessEmployee);

                return true;
            }
            catch
            {
                return false;
            }
        }

        [NonAction]
        public RegisterViewModel GetAdminSession()
        {
            try
            {
                RegisterViewModel = new RegisterViewModel()
                {
                    Business = new BusinessViewModel(),
                    Employee = new BusinessEmployeeViewModel(),
                    ServiceLocation = new ServiceLocationViewModel()
                };
                RegisterViewModel.Employee.ServiceLocation = new ServiceLocationViewModel();
                if (Request.Cookies[httpCookieKey] != null)
                {
                    AdminCookie = HttpContext.Request.Cookies[httpCookieKey];
                    if (AdminCookie != null)
                    {
                        RegisterViewModel.Employee.FirstName = AdminCookie.Values["aFirstName"];
                        RegisterViewModel.Employee.LastName = AdminCookie.Values["aLastName"];
                        RegisterViewModel.Employee.Email = AdminCookie.Values["aEmail"];
                        RegisterViewModel.Employee.IsActive = Convert.ToBoolean(AdminCookie.Values["aIsActive"]);
                        RegisterViewModel.Employee.IsAdmin = Convert.ToBoolean(AdminCookie.Values["aIsAdmin"]);
                        RegisterViewModel.Employee.Id = Convert.ToInt64(AdminCookie.Values["aEmpId"]);
                        RegisterViewModel.Business.Id = Convert.ToInt64(AdminCookie.Values["aBusinessId"]);
                        RegisterViewModel.Business.TimezoneId = Convert.ToInt32(AdminCookie.Values["aTimezoneId"]);
                        RegisterViewModel.Business.BusinessCategoryId = Convert.ToInt32(AdminCookie.Values["aBusinessCategoryId"]);
                        RegisterViewModel.Business.MembershipId = Convert.ToInt32(AdminCookie.Values["aMembershipId"]);
                        RegisterViewModel.Employee.ServiceLocationId = Convert.ToInt64(AdminCookie.Values["aServiceLocationId"]);
                        RegisterViewModel.ServiceLocation.Name = AdminCookie.Values["aServiceLocationName"];
                        Token = AdminCookie.Values["aToken"];
                        RegisterViewModel.Business.tblMembership.TotalOffers = Convert.ToInt32(AdminCookie.Values["aTotalOffers"]);
                        RegisterViewModel.Business.tblMembership.IsUnlimited = Convert.ToBoolean(AdminCookie.Values["aIsUnlimited"]);
                        RegisterViewModel.Business.tblMembership.TotalCustomer = Convert.ToInt32(AdminCookie.Values["aTotalCustomer"]);
                        RegisterViewModel.Business.tblMembership.TotalEmployee = Convert.ToInt32(AdminCookie.Values["aTotalEmployee"]);
                        RegisterViewModel.Business.tblMembership.TotalLocation = Convert.ToInt32(AdminCookie.Values["aTotalLocation"]);
                        RegisterViewModel.Business.tblMembership.TotalAppointment = Convert.ToInt32(AdminCookie.Values["aTotalAppointment"]);
                        return RegisterViewModel;
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
        public bool SetSessionValueByName(string name, string value)
        {
            try
            {
                if (Request.Cookies[httpCookieKey] != null)
                {
                    var businessEmployee = HttpContext.Request.Cookies[httpCookieKey];
                    businessEmployee.Values[name] = value;
                    Response.Cookies.Set(businessEmployee);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        [NonAction]
        protected bool LoginStatus()
        {
            try
            {
                RegisterViewModel = GetAdminSession();
                return (RegisterViewModel != null) ? true : false;
            }
            catch
            {
                return false;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!LoginStatus())
                filterContext.Result = RedirectToAction("login", "home", new { area = "employee" });
        }
    }
}