using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class BusinessAdminController : AdminBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var result = await BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
            if (result != null && result.Status)
                return View(result);
            else
            {
                var response = this.ResponseHelper.GetResponse<BusinessEmployeeViewModel>();
                return View(Response);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Edit()
        {
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
                    ServiceLocationId = result.Data.ServiceLocationId,
                    IsAdmin = result.Data.IsAdmin
                };
                response.Data = updateModel;
                response.Message = result.Message;
                response.Status = result.Status;
            }
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")] ResponseViewModel<BusinessEmployeeUpdateViewModel> model)
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
                        ServiceLocationId = model.Data.ServiceLocationId,
                        IsAdmin = model.Data.IsAdmin
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
        public async Task<ActionResult> Security()
        {
            var response = this.ResponseHelper.GetResponse<BusinessEmployeeUpdateSecurityViewModel>();
            var result = await BusinessEmployeeService.Get(RegisterViewModel.Employee.Id);
            if (result != null && result.Status)
            {
                var updateModel = new BusinessEmployeeUpdateSecurityViewModel()
                {
                    Id = result.Data.Id,
                    Email = result.Data.Email
                };
                response.Data = updateModel;
                response.Message = result.Message;
                response.Status = result.Status;
            }
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Security([Bind(Include = "Data")] ResponseViewModel<BusinessEmployeeUpdateSecurityViewModel> model)
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