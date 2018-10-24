using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Schedule.Web.Helpers
{
    public static class AppointmentHelper
    {

        public static List<SelectListItem> GetListofZipcode()
        {
            return new List<SelectListItem>()
                {
                    new SelectListItem() { Text = "1202", Value = "1202" },
                    new SelectListItem() { Text = "1200", Value = "1200" },
                    new SelectListItem() { Text = "1203", Value = "1203" },
                    new SelectListItem() { Text = "1401", Value = "1401" },
                    new SelectListItem() { Text = "1301", Value = "1301" },
                };
        }

        public static List<SelectListItem> GetListofState()
        {
            return new List<SelectListItem>()
                {
                    new SelectListItem(){ Text ="Advance", Value="Advance" },
                    new SelectListItem(){ Text ="Alamo", Value="Alamo" },
                    new SelectListItem(){ Text ="Amo", Value="Amo" },
                    new SelectListItem(){ Text ="Arcadia", Value="Arcadia" },
                    new SelectListItem(){ Text ="Atlanta", Value="Atlanta" },
                };
        }
    }
}