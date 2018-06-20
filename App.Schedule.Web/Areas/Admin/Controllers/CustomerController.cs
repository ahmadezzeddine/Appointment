using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class CustomerController : CustomerBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<BusinessCustomerViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;
            ViewBag.Total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalCustomer;

            var result = await BusinessCustomerService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (result.Status)
            {
                var data = result.Data;
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<BusinessCustomerViewModel>(pageNumber, 5);
                }
                else
                {
                    model.Data = data.Where(d => d.FirstName.ToLower().Contains(search.ToLower()) || d.LastName.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
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
            var response = this.ResponseHelper.GetResponse<BusinessCustomerViewModel>();
            response.Data = new BusinessCustomerViewModel();
            response.Data.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;
            response.Status = true;
            response.Data.IsActive = true;

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
        public async Task<ActionResult> Add([Bind(Include = "Data")]ResponseViewModel<BusinessCustomerViewModel> model)
        {
            var result = new ResponseViewModel<BusinessCustomerViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
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
                    var response = await this.BusinessCustomerService.Add(model.Data);
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
            var employeUpdateViewModel = new ResponseViewModel<BusinessCustomerUpdateViewModel>();
            if (id.HasValue && locationid.HasValue)
            {
                ViewBag.CustomerId = id.Value;
                var response = await this.BusinessCustomerService.Get(id.Value);
                employeUpdateViewModel.Data = new BusinessCustomerUpdateViewModel();
                employeUpdateViewModel.Status = response.Status;
                employeUpdateViewModel.Message = response.Message;
                if (response != null)
                {
                    if (response.Status)
                    {
                        if (response.Data != null)
                        {
                            employeUpdateViewModel.Data = new BusinessCustomerUpdateViewModel()
                            {
                                Id = response.Data.Id,
                                FirstName = response.Data.FirstName,
                                LastName = response.Data.LastName,
                                Email = response.Data.Email,
                                StdCode = response.Data.StdCode,
                                PhoneNumber = response.Data.PhoneNumber,
                                Add1 = response.Data.Add1,
                                Add2 = response.Data.Add2,
                                City = response.Data.City,
                                State = response.Data.State,
                                Zip = response.Data.Zip,
                                ServiceLocationId = response.Data.ServiceLocationId
                            };
                        }
                        var ServiceLocations = await this.GetServiceLocations();
                        ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
                        {
                            Value = Convert.ToString(s.Id),
                            Text = s.Name,
                            Selected = s.Id == locationid.Value ? true : false
                        });

                        employeUpdateViewModel.Status = true;
                        return View(employeUpdateViewModel);
                    }

                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")]ResponseViewModel<BusinessCustomerUpdateViewModel> model)
        {
            var result = new ResponseViewModel<BusinessCustomerUpdateViewModel>();
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var businessEmployeeModel = new BusinessCustomerViewModel()
                {
                    Id = model.Data.Id,
                    FirstName = model.Data.FirstName,
                    LastName = model.Data.LastName,
                    Email = model.Data.Email,
                    StdCode = model.Data.StdCode,
                    PhoneNumber = model.Data.PhoneNumber,
                    Add1 = model.Data.Add1,
                    Add2 = model.Data.Add2,
                    City = model.Data.City,
                    State = model.Data.State,
                    Zip = model.Data.Zip,
                    ServiceLocationId = model.Data.ServiceLocationId
                };

                var response = await this.BusinessCustomerService.Update(businessEmployeeModel);
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
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Deactive(long? id)
        {
            if (id.HasValue)
            {
                var response = await this.BusinessCustomerService.Get(id.Value);

                if (response != null)
                {
                    if (response.Status)
                    {
                        response.Status = true;
                        return View(response);
                    }

                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactive([Bind(Include = "Data")]ResponseViewModel<BusinessCustomerViewModel> model)
        {
            var result = new ResponseViewModel<BusinessCustomerViewModel>();
            var response = await this.BusinessCustomerService.Deactive(model.Data.Id, model.Data.IsActive);
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
                var response = await this.BusinessCustomerService.Get(id.Value);

                if (response != null)
                {
                    if (response.Status)
                    {
                        response.Status = true;
                        return View(response);
                    }

                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")]ResponseViewModel<BusinessCustomerViewModel> model)
        {
            var result = new ResponseViewModel<BusinessCustomerViewModel>();
            var response = await this.BusinessCustomerService.Delete(model.Data.Id);
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