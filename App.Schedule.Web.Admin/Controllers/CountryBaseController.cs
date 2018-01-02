using App.Schedule.Services;
using System.Web.Mvc;

namespace App.Schedule.Web.Admin.Controllers
{
    public class CountryBaseController : BaseController
    {
        protected CountryService CountryService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = this.LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("Login", "Home", new { area = "Admin" });
            }
            else
            {
                this.CountryService = new CountryService(this.Token);
            }
        }
    }
}