using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class AppointmentBaseController : BaseController
    {
        protected AppointmentService AppointmentService;
        protected ServiceLocationService ServiceLocationService;
        protected BusinessCustomerService BusinessCustomerService;
        protected BusinessEmployeeService BusinessEmployeeService;
        protected BusinessOfferService BusinessOfferService;
        protected BusinessService BusinessService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("login", "home", new { area = "admin" });
            }
            else
            {
                this.AppointmentService = new AppointmentService(this.Token);
                this.ServiceLocationService = new ServiceLocationService(this.Token);
                this.BusinessCustomerService = new BusinessCustomerService(this.Token);
                this.BusinessOfferService = new BusinessOfferService(this.Token);
                this.BusinessEmployeeService = new BusinessEmployeeService(this.Token);
                this.BusinessService = new BusinessService(this.Token);
            }
        }
    }
}