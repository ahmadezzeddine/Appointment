using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Customer.Controllers.Base;
using System;

namespace App.Schedule.Web.Areas.Customer.Controllers
{
    public class AccountController : AccountBaseController
    {
        public async Task<ActionResult> Index()
        {
            var model = await this.BusinessCustomerService.Get(RegisterCustomerViewModel.Customer.Id);
            if(model.Data!=null && model.Data.ServiceLocation == null)
            {
                var serviceLocation = await this.ServiceLocationService.Get(model.Data.ServiceLocationId);
                if (serviceLocation != null && serviceLocation.Data !=null)
                {
                    model.Data.ServiceLocation = serviceLocation.Data;
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Update()
        {
            var updateModel = new ResponseViewModel<BusinessCustomerUpdateViewModel>();
            updateModel.Status = false;
            try
            {
                var model = await this.BusinessCustomerService.Get(RegisterCustomerViewModel.Customer.Id);
                if (model.Data != null && model.Data.ServiceLocation == null)
                {
                    updateModel.Status = model.Status;
                    updateModel.Data = new BusinessCustomerUpdateViewModel()
                    {
                        Id = model.Data.Id,
                        Add1 = model.Data.Add1,
                        Add2 = model.Data.Add2,
                        City = model.Data.City,
                        Email = model.Data.Email,
                        FirstName = model.Data.FirstName,
                        LastName = model.Data.FirstName,
                        PhoneNumber = model.Data.PhoneNumber,
                        ProfilePicture = model.Data.ProfilePicture,
                        State = model.Data.State,
                        StdCode = model.Data.StdCode,
                        Zip = model.Data.Zip,
                        ServiceLocationId = model.Data.ServiceLocationId
                    };
                    var serviceLocation = await this.ServiceLocationService.Get(model.Data.ServiceLocationId);
                    if (serviceLocation != null && serviceLocation.Data != null)
                    {
                        updateModel.Data.ServiceLocation = serviceLocation.Data;
                    }
                }
            }
            catch
            {
                updateModel.Status = false;
                updateModel.Message = "Please try again later.";
            }
            return View(updateModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([Bind(Include = "Data")] ResponseViewModel<BusinessCustomerUpdateViewModel> model)
        {
            var result = new ResponseViewModel<BusinessCustomerViewModel>();
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
                    var customerModel = new BusinessCustomerViewModel()
                    {
                        Id = model.Data.Id,
                        Add1 = model.Data.Add1,
                        Add2 = model.Data.Add2,
                        City = model.Data.City,
                        Email = model.Data.Email,
                        FirstName = model.Data.FirstName,
                        LastName = model.Data.LastName,
                        PhoneNumber = model.Data.PhoneNumber,
                        ProfilePicture = model.Data.ProfilePicture,
                        State = model.Data.State,
                        StdCode = model.Data.StdCode,
                        Zip = model.Data.Zip,
                        ServiceLocationId = model.Data.ServiceLocationId
                    };
                    var response = await this.BusinessCustomerService.Update(customerModel);
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
            var model = await this.BusinessCustomerService.Get(RegisterCustomerViewModel.Customer.Id);
            if (model.Data != null && model.Data.ServiceLocation == null)
            {
                model.Data.Password = "";
                var serviceLocation = await this.ServiceLocationService.Get(model.Data.ServiceLocationId);
                if (serviceLocation != null && serviceLocation.Data != null)
                {
                    model.Data.ServiceLocation = serviceLocation.Data;
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Password([Bind(Include = "Data")] ResponseViewModel<BusinessCustomerViewModel> model)
        {
            var result = new ResponseViewModel<BusinessCustomerViewModel>();
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
                    var response = await this.BusinessCustomerService.Get(RegisterCustomerViewModel.Customer.Id);
                    if (response.Status)
                    {
                        response.Data.Password = model.Data.Password;
                        response.Data.OldPassword = model.Data.OldPassword;
                        response.Data.ConfirmPassword = model.Data.ConfirmPassword;
                        var getUserResponse = await this.BusinessCustomerService.Update(response.Data,true);
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