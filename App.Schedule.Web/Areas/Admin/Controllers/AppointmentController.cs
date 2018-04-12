using App.Schedule.Domains.ViewModel;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class AppointmentController : AppointmentBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (result.Status)
            {
                var data = result.Data;
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 5);
                }
                else
                {
                    model.Data = data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
            }
            return View(model);
        }

    }
}