using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Admin/Employee
        public ActionResult Index()
        {
            return View();
        }
    }
}