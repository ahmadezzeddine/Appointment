using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Admin.Controllers
{
    public class RegisterController : RegisterBaseController
    {
        public ActionResult Index()
        {
            var model = new AdministratorViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Index(AdministratorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                return Json(new { status = false, message = errMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var verifyUser = await this.AdminService.VerifyByEmail(model.Email);
                if (verifyUser != null && !verifyUser.Status)
                {
                    var response = await this.AdminService.Add(model);
                    if (response.Status)
                    {
                        return Json(new { status = true, message = "Successfully registered." }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "There was a problem. Please try again later."}, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { status = false, message = "The email address you have entered is already registered. Did you forgot your login information?" }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}