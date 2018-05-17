using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Customer.Controllers.Base;

namespace App.Schedule.Web.Areas.Customer.Controllers
{
    public class HomeController : HomeBaseController
    {
        [HttpGet]
        public ActionResult Login()
        {
            var model = new ResponseViewModel<LoginViewModel>();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Login(ResponseViewModel<LoginViewModel> model)
        {
            var result = new ResponseViewModel<RegisterCustomerViewModel>();
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
                    if (BusinessCustomerService != null)
                    {
                        var response = await BusinessCustomerService.VerifyLoginCredential(model.Data.Email, model.Data.Password);
                        result.Status = response.Status;
                        result.Message = response.Message;
                        result.Data = response.Data;
                        if (response.Status)
                        {
                            var tokenResponse = await BusinessCustomerService.VerifyAngGetAccessToken(model.Data.Email, model.Data.Password);
                            result.Status = tokenResponse.Status;
                            result.Message = tokenResponse.Message;
                            if (tokenResponse.Status)
                            {
                                if (string.IsNullOrEmpty(tokenResponse.Data))
                                {
                                    RedirectToAction("Logout", "Dashboard", new { area = "Customer" });
                                }
                                SetAdminSession(response.Data, model.Data.IsKeepLoggedIn, tokenResponse.Data);
                            }
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "There was a problem. Please try again later.";
                    }
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message, data = result.Data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Forgot()
        {
            return View();
        }
    }
}