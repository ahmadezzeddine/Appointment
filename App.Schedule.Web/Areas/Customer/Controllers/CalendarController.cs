﻿using System.Web.Mvc;
using App.Schedule.Web.Areas.Customer.Controllers.Base;

namespace App.Schedule.Web.Areas.Customer.Controllers
{
    public class CalendarController : CalendarBaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Week()
        {
            return View();
        }
        public ActionResult Timeline()
        {
            return View();
        }
    }
}