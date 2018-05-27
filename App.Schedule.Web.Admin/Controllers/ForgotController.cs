using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Schedule.Web.Admin.Models;

namespace App.Schedule.Web.Admin.Controllers
{
    public class ForgotController : LoginBaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(ForgotViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                return Json(new { status = false, message = errMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var response = await this.AdminService.VerifyLoginCredential(model.Email,"", true);
                if (response != null)
                {
                    return Json(new { status = response.Status, model = "", message = response.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false, message = "There was a problem. Please try again later." }, JsonRequestBehavior.AllowGet);
        }
    }
}