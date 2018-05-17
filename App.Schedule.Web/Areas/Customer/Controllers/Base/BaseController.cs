using System;
using System.Web;
using System.Web.Mvc;
using App.Schedule.Web.Helpers;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Customer.Controllers.Base
{
    public class BaseController : Controller
    {
        protected string Token;
        protected HttpCookie CustomerCookie;
        protected ResponseHelper ResponseHelper;
        protected RegisterCustomerViewModel RegisterCustomerViewModel;
        public const string httpCookieKey = "acustomerappointment";

        public BaseController()
        {
            this.ResponseHelper = new ResponseHelper();
            this.RegisterCustomerViewModel = new RegisterCustomerViewModel();
        }

        [NonAction]
        public bool SetAdminSession(RegisterCustomerViewModel model, bool isKeepLoggedIn, string token)
        {
            try
            {
                Session["aEmail"] = model.Customer.Email;
                var customerSession = new HttpCookie(httpCookieKey);

                if (isKeepLoggedIn)
                    customerSession.Expires = DateTime.Now.AddDays(365);
                else
                    customerSession.Expires = DateTime.Now.AddDays(1);

                customerSession.Values["aToken"] = token;
                customerSession.Values["aEmail"] = model.Customer.Email;
                customerSession.Values["aFirstName"] = model.Customer.FirstName;
                customerSession.Values["aLastName"] = model.Customer.LastName;
                customerSession.Values["aCustomerId"] = Convert.ToString(model.Customer.Id);
                customerSession.Values["aIsActive"] = model.Customer.IsActive ? "true" : "false";
                customerSession.Values["aBusinessId"] = Convert.ToString(model.ServiceLocation.BusinessId);
                customerSession.Values["aTimezoneId"] = Convert.ToString(model.ServiceLocation.TimezoneId);
                customerSession.Values["aServiceLocationId"] = Convert.ToString(model.Customer.ServiceLocationId);
                customerSession.Values["aBusinessName"] = string.Format("{0} ({1})", model.Business.Name, model.Business.ShortName);
                customerSession.Values["aServiceLocationName"] = model.ServiceLocation.Name;

                Response.Cookies.Add(customerSession);

                return true;
            }
            catch
            {
                return false;
            }
        }

        [NonAction]
        public RegisterCustomerViewModel GetCustomerSession()
        {
            try
            {
                if (Request.Cookies[httpCookieKey] != null)
                {
                    RegisterCustomerViewModel.ServiceLocation = new ServiceLocationViewModel();
                    RegisterCustomerViewModel.Business = new BusinessViewModel();
                    RegisterCustomerViewModel.Customer = new BusinessCustomerViewMdoel();
                    CustomerCookie = HttpContext.Request.Cookies[httpCookieKey];
                    if (CustomerCookie != null)
                    {
                        RegisterCustomerViewModel.Customer.FirstName = CustomerCookie.Values["aFirstName"];
                        RegisterCustomerViewModel.Customer.LastName = CustomerCookie.Values["aLastName"];
                        RegisterCustomerViewModel.Customer.Email = CustomerCookie.Values["aEmail"];
                        RegisterCustomerViewModel.Customer.IsActive = Convert.ToBoolean(CustomerCookie.Values["aIsActive"]);
                        RegisterCustomerViewModel.Customer.Id = Convert.ToInt64(CustomerCookie.Values["aCustomerId"]);
                        RegisterCustomerViewModel.ServiceLocation.BusinessId = Convert.ToInt64(CustomerCookie.Values["aBusinessId"]);
                        RegisterCustomerViewModel.Business.Id = Convert.ToInt64(CustomerCookie.Values["aBusinessId"]);
                        RegisterCustomerViewModel.ServiceLocation.TimezoneId = Convert.ToInt32(CustomerCookie.Values["aTimezoneId"]);
                        RegisterCustomerViewModel.Customer.ServiceLocationId = Convert.ToInt64(CustomerCookie.Values["aServiceLocationId"]);
                        RegisterCustomerViewModel.ServiceLocation.Id = Convert.ToInt64(CustomerCookie.Values["aServiceLocationId"]);
                        RegisterCustomerViewModel.ServiceLocation.Name = Convert.ToString(CustomerCookie.Values["aServiceLocationName"]);
                        Token = CustomerCookie.Values["aToken"];
                        return RegisterCustomerViewModel;
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
                RegisterCustomerViewModel = GetCustomerSession();
                //Call service;
                if (RegisterCustomerViewModel != null)
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
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Customer" });
        }
    }
}