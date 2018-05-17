using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Customer.Controllers.Base
{
    public class AccountBaseController : BaseController
    {
        protected BusinessCustomerService BusinessCustomerService;
        protected ServiceLocationService ServiceLocationService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("login", "home", new { area = "customer" });
            }
            else
            {
                this.BusinessCustomerService = new BusinessCustomerService(this.Token);
                this.ServiceLocationService = new ServiceLocationService(this.Token);
            }
        }
    }
}