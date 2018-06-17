using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Employee.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Employee.Controllers
{
    public class AccountController : AccountBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var model = await this.BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
            if (model.Data != null && model.Data.ServiceLocation == null)
            {
                var serviceLocation = await this.ServiceLocationService.Get(model.Data.ServiceLocationId);
                if (serviceLocation != null && serviceLocation.Data != null)
                {
                    model.Data.ServiceLocation = serviceLocation.Data;
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Update()
        {
            //var model = await this.BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
            //if (model.Data != null && model.Data.ServiceLocation == null)
            //{
            //    var serviceLocation = await this.ServiceLocationService.Get(model.Data.ServiceLocationId);
            //    if (serviceLocation != null && serviceLocation.Data != null)
            //    {
            //        model.Data.ServiceLocation = serviceLocation.Data;
            //    }
            //}
            //return View(model);

            var response = this.ResponseHelper.GetResponse<BusinessEmployeeUpdateViewModel>();
            var result = await BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
            if (result != null && result.Status)
            {
                var updateModel = new BusinessEmployeeUpdateViewModel()
                {
                    Email = result.Data.Email,
                    FirstName = result.Data.FirstName,
                    Id = result.Data.Id,
                    LastName = result.Data.LastName,
                    PhoneNumber = result.Data.PhoneNumber,
                    STD = result.Data.STD,
                    IsAdmin = result.Data.IsAdmin,
                    ServiceLocationId = result.Data.ServiceLocationId
                };
                response.Data = updateModel;
                response.Message = result.Message;
                response.Status = result.Status;
            }
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([Bind(Include = "Data")] ResponseViewModel<BusinessEmployeeUpdateViewModel> model)
        {
            var result = new ResponseViewModel<BusinessEmployeeUpdateViewModel>();
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
                    var adminInformation = new BusinessEmployeeViewModel()
                    {
                        FirstName = model.Data.FirstName,
                        LastName = model.Data.LastName,
                        Email = model.Data.Email,
                        Id = model.Data.Id,
                        PhoneNumber = model.Data.PhoneNumber,
                        STD = model.Data.STD,
                        IsAdmin = model.Data.IsAdmin,
                        ServiceLocationId = model.Data.ServiceLocationId
                    };
                    var response = await this.BusinessEmployeeService.Update(adminInformation);
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
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Password()
        {
            var res = new ResponseViewModel<BusinessEmployeeUpdateSecurityViewModel>();
            try
            {
                var model = await this.BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
                if (model.Data != null)
                {
                    res.Data = new BusinessEmployeeUpdateSecurityViewModel()
                    {
                        Id = model.Data.Id,
                        Email = model.Data.Email,
                    };
                }
                res.Status = true;
            }
            catch
            {
                res.Status = false;
                res.Message = "There was a problem. please try again later.";
            }
            return View(res);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Password([Bind(Include = "Data")] ResponseViewModel<BusinessEmployeeUpdateSecurityViewModel> model)
        {
            var result = new ResponseViewModel<BusinessEmployeeUpdateSecurityViewModel>();
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
                    var response = await this.BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
                    if (response.Status)
                    {
                        response.Data.Password = model.Data.Password;
                        response.Data.OldPassword = model.Data.OldPassword;
                        response.Data.ConfirmPassword = model.Data.ConfirmPassword;
                        var getUserResponse = await this.BusinessEmployeeService.Update(response.Data, false);
                        if (getUserResponse.Status)
                        {
                            result.Status = true;
                            result.Message = getUserResponse.Message;
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = getUserResponse.Message;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = response.Message;
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
    }
}