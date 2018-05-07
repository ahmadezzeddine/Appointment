using App.Schedule.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class DocumentCategoryBaseController : BaseController
    {
        protected DocumentCategoryService DocumentCategoryService;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var status = LoginStatus();
            if (!status)
            {
                filterContext.Result = RedirectToAction("login", "home", new { area = "admin" });
            }
            else
            {
                this.DocumentCategoryService = new DocumentCategoryService(this.Token);
            }
        }
    }
}