using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Models;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class HomeController : HomeBaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            var password = Domains.Helpers.Security.Decrypt("8xNmErUPwsqfOZJ/+l2bvA==", true);
            var model = new ResponseViewModel<LoginViewModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(ResponseViewModel<LoginViewModel> model)
        {
            var result = new ResponseViewModel<RegisterViewModel>();
            result.Status = false;
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
                     if (BusinessEmployeeService != null)
                    {
                        var response = await BusinessEmployeeService.VerifyLoginCredential(model.Data.Email, model.Data.Password, false);
                        result.Status = response.Status;
                        result.Message = response.Message;
                        result.Data = response.Data;
                        if (response!=null && response.Status)
                        {
                            if (response.Data.Employee.IsAdmin)
                            {
                                var tokenResponse = await BusinessEmployeeService.VerifyAndGetAdminAccessToken(model.Data.Email, model.Data.Password);
                                result.Status = tokenResponse.Status;
                                result.Message = tokenResponse.Message;
                                if (tokenResponse.Status)
                                {
                                    if (string.IsNullOrEmpty(tokenResponse.Data))
                                    {
                                        RedirectToAction("Logout", "Dashboard", new { area = "Admin" });
                                    }
                                    SetAdminSession(response.Data, model.Data.IsKeepLoggedIn, tokenResponse.Data);
                                }
                            }
                            else
                            {
                                result.Message = "You cann't login as Admin, Please try login as employee.";
                                result.Status = false;
                                result.Data = null;
                            }
                        }
                        else
                        {
                            result.Message = response.Message;
                            result.Status = false;
                            result.Data = null;
                        }
                    }
                    else
                    {
                        result.Message = "There was a problem. Please try again later.";
                    }
                }
            }
            catch
            {
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message, data = result.Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Forgot(ForgotViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                return Json(new { status = false, message = errMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = await this.BusinessEmployeeService.VerifyLoginCredential(model.Email, "", true);
                if (response != null)
                {
                    return Json(new { status = response.Status, model = "", message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false, message = "There was a problem. Please try again later." }, JsonRequestBehavior.AllowGet);
        }
    }
}