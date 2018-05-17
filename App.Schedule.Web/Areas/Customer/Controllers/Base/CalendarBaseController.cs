using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Customer.Controllers.Base
{
    public class CalendarBaseController : BaseController
    {
        protected AppointmentService AppointmentService;
        protected AppointmentDocumentService AppointmentDocumentService;
        protected AppointmentFeedbackService AppointmentFeedbackService;
        protected AppointmentInviteeService AppointmentInviteeService;
        protected DocumentCategoryService DocumentCategoryService;
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
                filterContext.Result = RedirectToAction("login", "home", new { area = "customer" });
            }
            else
            {
                this.BusinessService = new BusinessService(this.Token);
                this.AppointmentService = new AppointmentService(this.Token);
                this.AppointmentInviteeService = new AppointmentInviteeService(this.Token);
                this.BusinessOfferService = new BusinessOfferService(this.Token);
                this.ServiceLocationService = new ServiceLocationService(this.Token);
                this.DocumentCategoryService = new DocumentCategoryService(this.Token);
                this.BusinessCustomerService = new BusinessCustomerService(this.Token);
                this.BusinessEmployeeService = new BusinessEmployeeService(this.Token);
                this.AppointmentDocumentService = new AppointmentDocumentService(this.Token);
                this.AppointmentFeedbackService = new AppointmentFeedbackService(this.Token);
            }
        }
    }
}