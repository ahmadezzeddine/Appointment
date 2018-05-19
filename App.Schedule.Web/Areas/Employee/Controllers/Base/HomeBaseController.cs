using System.Web.Mvc;
using App.Schedule.Web.Services;

namespace App.Schedule.Web.Areas.Employee.Controllers.Base
{
    public class HomeBaseController : BaseController
    {
        protected BusinessEmployeeService BusinessEmployeeService;
        protected BusinessCategoryService BusinessCategoryService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (status)
            {
                filterContext.Result = RedirectToAction("index", "dashboard", new { area = "employee" });
            }
            else
            {
                this.BusinessEmployeeService = new BusinessEmployeeService(this.Token);
                this.BusinessCategoryService = new BusinessCategoryService(this.Token);
            }
        }
    }
}