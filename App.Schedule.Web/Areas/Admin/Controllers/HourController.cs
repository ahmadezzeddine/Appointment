using App.Schedule.Domains.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class HourController : Controller
    {
        // GET: Admin/Hour
        public ActionResult Index()
        {
            var model = new ResponseViewModel<List<BusinessHourViewModel>>();
            model.Status = true;
            model.Data = new List<BusinessHourViewModel>();
            return View(model);
        }

        public ActionResult Edit()
        {
            var model = new ResponseViewModel<List<BusinessHourViewModel>>();
            model.Status = true;
            model.Data = new List<BusinessHourViewModel>();
            return View(model);
        }

    }
}