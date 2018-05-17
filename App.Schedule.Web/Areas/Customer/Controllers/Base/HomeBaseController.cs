using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Customer.Controllers.Base
{
    public class HomeBaseController : BaseController
    {
        protected BusinessCustomerService BusinessCustomerService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (status)
            {
                filterContext.Result = RedirectToAction("Index", "Dashboard", new { area = "Customer" });
            }
            else
            {
                this.BusinessCustomerService = new BusinessCustomerService(this.Token);
            }
        }
    }
}