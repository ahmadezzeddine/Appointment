using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class EmployeeController : EmployeeBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<BusinessEmployeeViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            model.hasAdd = true;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.LoggedEmployeeEmailId = RegisterViewModel.Employee.Email;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalEmployee;
            var result = await BusinessEmployeeService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (result.Status)
            {
                var data = result.Data;
                model.hasAdd = result.Data != null && result.Data.Count <= total ? true : false;
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<BusinessEmployeeViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.FirstName.ToLower().StartsWith(search.ToLower()) || d.LastName.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            var response = this.ResponseHelper.GetResponse<BusinessEmployeeViewModel>();
            response.Data = new BusinessEmployeeViewModel();
            response.Data.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;
            response.Status = true;

            var ServiceLocations = await this.GetServiceLocations();
            ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = s.Name
            });

            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "Data")]ResponseViewModel<BusinessEmployeeViewModel> model)
        {
            var result = new ResponseViewModel<BusinessEmployeeViewModel>();

            if (!ModelState.IsValid)
            {
                result.Status = false;
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Message = errMessage;
            }
            else
            {
                if (model.Data.Password.Length <= 8)
                {
                    result.Status = false;
                    result.Message = "Password must be greater than 8 character.";
                }
                else
                {
                    var response = await this.BusinessEmployeeService.Add(model.Data);
                    if (response == null)
                    {
                        result.Status = false;
                        result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                    }
                    else
                    {
                        result.Status = response.Status;
                        result.Message = response.Message;
                    }
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long? id, long? locationid)
        {
            if (id.HasValue && locationid.HasValue)
            {
                ViewBag.EmployeeId = id.Value;
                var response = await this.BusinessEmployeeService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        var model = new ResponseViewModel<BusinessEmployeeUpdateViewModel>();
                        model.Data = new BusinessEmployeeUpdateViewModel()
                        {
                            Email = response.Data.Email,
                            FirstName = response.Data.FirstName,
                            Id = response.Data.Id,
                            IsAdmin = response.Data.IsAdmin,
                            LastName = response.Data.LastName,
                            PhoneNumber = response.Data.PhoneNumber,
                            ServiceLocationId = response.Data.ServiceLocationId,
                            STD = response.Data.STD,
                            LoggedUser = RegisterViewModel
                        };

                        var ServiceLocations = await this.GetServiceLocations();
                        ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
                        {
                            Value = Convert.ToString(s.Id),
                            Text = s.Name,
                            Selected = s.Id == locationid.Value ? true : false
                        });

                        model.Status = true;
                        return View(model);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")]ResponseViewModel<BusinessEmployeeUpdateViewModel> model)
        {
            var result = new ResponseViewModel<BusinessEmployeeUpdateViewModel>();
            try
            {
                if (!ModelState.IsValid)
                {
                    result.Status = false;
                    var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                    result.Message = errMessage;
                    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var employeeUpdateModel = new BusinessEmployeeViewModel()
                    {
                        Email = model.Data.Email,
                        FirstName = model.Data.FirstName,
                        Id = model.Data.Id,
                        IsAdmin = model.Data.IsAdmin,
                        LastName = model.Data.LastName,
                        PhoneNumber = model.Data.PhoneNumber,
                        ServiceLocationId = model.Data.ServiceLocationId,
                        STD = model.Data.STD,
                        LoggedUser = RegisterViewModel
                    };
                    var response = await this.BusinessEmployeeService.Update(employeeUpdateModel);
                    if (response == null)
                    {
                        result.Status = false;
                        result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                    }
                    else
                    {
                        result.Status = response.Status;
                        result.Message = response.Message;
                    }
                    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { status = false, message = "There was a problem. Please try again later." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public async Task<ActionResult> Deactive(long? id)
        {
            if (id.HasValue)
            {
                var response = await this.BusinessEmployeeService.Get(id.Value);

                if (response != null)
                {
                    if (response.Status && response.Data!=null)
                    {
                        if (response.Data.Email.ToLower() != RegisterViewModel.Employee.Email)
                        {
                            return View(response);
                        }
                    }

                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactive([Bind(Include = "Data")]ResponseViewModel<BusinessEmployeeViewModel> model)
        {
            var result = new ResponseViewModel<BusinessEmployeeViewModel>();
            var response = await this.BusinessEmployeeService.Deactive(model.Data.Id, model.Data.IsActive);
            if (response == null)
            {
                result.Status = false;
                result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
            }
            else
            {
                result.Status = response.Status;
                result.Message = response.Message;
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(long? id)
        {
            if (id.HasValue)
            {
                var response = await this.BusinessEmployeeService.Get(id.Value);

                if (response != null)
                {
                    if (response.Status && response.Data != null)
                    {
                        if (response.Data.Email.ToLower() != RegisterViewModel.Employee.Email)
                        {
                            return View(response);
                        }
                    }

                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")]ResponseViewModel<BusinessEmployeeViewModel> model)
        {
            var result = new ResponseViewModel<BusinessEmployeeViewModel>();
            var response = await this.BusinessEmployeeService.DeleteEmployee(model.Data);
            if (response == null)
            {
                result.Status = false;
                result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
            }
            else
            {
                result.Status = response.Status;
                result.Message = response.Message;
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private async Task<List<ServiceLocationViewModel>> GetServiceLocations()
        {
            var response = await this.ServiceLocationService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<ServiceLocationViewModel>();
            }
        }

    }
}