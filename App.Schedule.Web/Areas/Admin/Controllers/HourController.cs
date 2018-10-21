using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Domains.Helpers;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class HourController : HourBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await this.BusinessHourService.Gets(RegisterViewModel.Employee.ServiceLocationId.Value, TableType.ServiceLocationId);
            model.Status = true;
            if (model.Data == null)
                model.Data = new List<BusinessHourViewModel>();
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "hour", new { area = "admin" });

            var model = await this.BusinessHourService.Get(id);
            var fromHours = Hour.GetHoursOfDay();
            model.Status = true;

            //ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            //{
            //    Value = s.Value,
            //    Text = s.Value
            //});
            //ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            //{
            //    Value = s.Value,
            //    Text = s.Value,
            //    Selected = DateTime.Parse(s.Value.ToString()) > DateTime.Parse(s.Value.ToString()) ? true : false
            //});

            var hours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value
            });

            //ViewBag.From = new SelectList(hours, "Value", "Text", SelectDate(hours,model.Data.From));
            //ViewBag.To = new SelectList(hours, "Value", "Text", SelectDate(hours,model.Data.To));

            //ViewBag.FromSplit1 = new SelectList(hours, "Value", "Text", model.Data.FromSplit1.HasValue ? SelectDate(hours, model.Data.FromSplit1.Value) : 1);
            //ViewBag.ToSplit1 = new SelectList(hours, "Value", "Text", model.Data.ToSplit1.HasValue ? SelectDate(hours, model.Data.ToSplit1.Value) : 1);

            //ViewBag.FromSplit2 = new SelectList(hours, "Value", "Text", model.Data.FromSplit2.HasValue ? SelectDate(hours, model.Data.FromSplit2.Value) : 1);
            //ViewBag.ToSplit2 = new SelectList(hours, "Value", "Text", model.Data.ToSplit2.HasValue ? SelectDate(hours, model.Data.ToSplit2.Value) : 1);

            ViewBag.From = hours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Text,
                Selected = s.Value == model.Data.From.ToString("hh:mm tt") ? true : false
            });
            ViewBag.To = hours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Text,
                Selected = s.Value == model.Data.To.ToString("hh:mm tt") ? true : false
            });

            ViewBag.FromSplit1 = hours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Text,
                Selected = model.Data.FromSplit1.HasValue && s.Value == model.Data.FromSplit1.Value.ToString("hh:mm tt") ? true : false
            });
            ViewBag.ToSplit1 = hours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Text,
                Selected = model.Data.ToSplit1.HasValue && s.Value == model.Data.ToSplit1.Value.ToString("hh:mm tt") ? true : false
            });

            ViewBag.FromSplit2 = hours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Text,
                Selected = model.Data.FromSplit2.HasValue && s.Value == model.Data.FromSplit2.Value.ToString("hh:mm tt") ? true : false
            });
            ViewBag.ToSplit2 = hours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Text,
                Selected = model.Data.ToSplit2.HasValue && s.Value == model.Data.ToSplit2.Value.ToString("hh:mm tt") ? true : false
            });
            return View(model);
        }

        public int SelectDate(IEnumerable<SelectListItem> hours, DateTime time)
        {
            var index = 0;
            foreach (var hour in hours.Select((data,i) =>  new { i, data }))
            {
                var timeString = time.ToString("hh:mm tt");
                if (hour.data.Value == timeString)
                {
                    index = hour.i;
                }
            }
            return index;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")] ResponseViewModel<BusinessHourViewModel> model)
        {
            var result = new ResponseViewModel<BusinessHourViewModel>();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                    result.Status = false;
                    result.Message = errMessage;
                }
                else
                {
                    if (CheckSplitDate(model.Data))
                    {
                        var response = await this.BusinessHourService.Update(model.Data);
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
                    else
                    {
                        result.Status = false;
                        result.Message = "Please validate your business time.";
                    }
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
        private bool CheckSplitDate(BusinessHourViewModel model)
        {
            if (model.IsHoliday)
                return true;

            if (model.From.Hour >= model.To.Hour)
                return false;

            if (model.IsSplit1.HasValue)
            {
                if (model.IsSplit1.Value)
                {
                    if (model.To.Hour > model.FromSplit1.Value.Hour)
                        return false;

                    if (model.FromSplit1.Value.Hour > model.ToSplit1.Value.Hour)
                        return false;
                }
            }

            if (model.IsSplit2.HasValue)
            {
                if (model.IsSplit2.Value)
                {
                    if (model.ToSplit1.Value.Hour > model.FromSplit2.Value.Hour)
                        return false;
                    if (model.FromSplit2.Value.Hour > model.ToSplit2.Value.Hour)
                        return false;
                }
            }
            return true;
        }

      
    }
}