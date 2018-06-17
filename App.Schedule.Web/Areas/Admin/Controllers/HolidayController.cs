using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class HolidayController : HolidayBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var result = await this.BusinessHolidayService.Gets(RegisterViewModel.Employee.ServiceLocationId.Value, TableType.ServiceLocationId);
            if (result != null && result.Status)
            {
                result.Data = result.Data.OrderByDescending(d => d.Id).ToList();
                return View(result);
            }
            else
            {
                var response = this.ResponseHelper.GetResponse<List<BusinessHolidayViewModel>>();
                return View(Response);
            }
        }

        [HttpGet]
        public ActionResult Add(long? servicelocationid)
        {
            var model = new ResponseViewModel<BusinessHolidayViewModel>();
            model.Data = new BusinessHolidayViewModel();
            if (servicelocationid.HasValue)
                model.Data.ServiceLocationId = servicelocationid.Value;
            else
                model.Data.ServiceLocationId = this.RegisterViewModel.Employee.ServiceLocationId;
            var holidayTypes = from RecureType e in Enum.GetValues(typeof(RecureType))
                               select new
                               {
                                   ID = (int)e,
                                   Name = e.ToString()
                               };
            ViewBag.HolidayType = new SelectList(holidayTypes, "Id", "Name");
            model.Status = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "Data")] ResponseViewModel<BusinessHolidayViewModel> model)
        {
            var result = new ResponseViewModel<BusinessHolidayViewModel>();
            try
            {
                if (ValidateDate(model.Data))
                {
                    if (!ModelState.IsValid)
                    {
                        var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                        result.Status = false;
                        result.Message = errMessage;
                    }
                    else
                    {

                        var response = await this.BusinessHolidayService.Add(model.Data);
                        if (response.Status)
                        {
                            result.Status = true;
                            result.Message = response.Message;
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = response.Message;
                        }
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = "Please validate your business time.";
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "holiday", new { area = "admin" });

            var model = await this.BusinessHolidayService.Get(id);
            var holidayTypes = from RecureType e in Enum.GetValues(typeof(RecureType))
                               select new
                               {
                                   ID = (int)e,
                                   Name = e.ToString()
                               };
            ViewBag.HolidayType = new SelectList(holidayTypes, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")] ResponseViewModel<BusinessHolidayViewModel> model)
        {
            var result = new ResponseViewModel<BusinessHolidayViewModel>();
            try
            {
                if (ValidateDate(model.Data))
                {
                    if (!ModelState.IsValid)
                    {
                        var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                        result.Status = false;
                        result.Message = errMessage;
                    }
                    else
                    {

                        var response = await this.BusinessHolidayService.Update(model.Data);
                        if (response.Status)
                        {
                            result.Status = true;
                            result.Message = response.Message;
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = response.Message;
                        }

                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = "Please validate your business time.";
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "holiday", new { area = "admin" });

            var model = await this.BusinessHolidayService.Get(id);
            var holidayTypes = from RecureType e in Enum.GetValues(typeof(RecureType))
                               select new
                               {
                                   ID = (int)e,
                                   Name = e.ToString()
                               };
            ViewBag.HolidayType = new SelectList(holidayTypes, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")] ResponseViewModel<BusinessHolidayViewModel> model)
        {
            var result = new ResponseViewModel<BusinessHolidayViewModel>();
            try
            {

                var response = await this.BusinessHolidayService.Delete(model.Data.Id);
                if (response.Status)
                {
                    result.Status = true;
                    result.Message = response.Message;
                }
                else
                {
                    result.Status = false;
                    result.Message = response.Message;
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private bool ValidateDate(BusinessHolidayViewModel model)
        {
            if (model != null)
            {
                if (model.OnDate.Year == 1)
                    return false;

                var date = new DateTime();
                var isValidate = DateTime.TryParse(model.OnDate.ToString(), out date);
                if (isValidate)
                    return true;
                else
                    return false;

            }
            return false;
        }
    }
}