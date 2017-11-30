using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Admin.Controllers;
using System.Threading.Tasks;

namespace App.Schedule.Web.Controllers
{
    public class HomeController : LoginBaseController
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Register()
        {
            var result = new ServiceDataViewModel<RegisterViewModel>();
            result.HasError = false;
            result.HasMore = false;
            try
            {
                var Countries = await this.GetCountries();
                ViewBag.CountryId = Countries.Select(s => new SelectListItem()
                {
                    Value = Convert.ToString(s.Id),
                    Text = s.Name
                });
                
                var BusinessCategories = await this.GetBusinessCategories();
                var parentCategories = BusinessCategories.ToDictionary(d => d.Id, d => d.Name);
                var groupCategories = BusinessCategories.Select(s => s.Name).Select(ss => new SelectListGroup() { Name = ss }).ToList();

                var childCategories = (from c in BusinessCategories
                                       join p in BusinessCategories
                                       on c.ParentId equals p.Id
                                       select new
                                       {
                                           Id = c.Id,
                                           Text = c.Name,
                                           ParentId = c.ParentId
                                       }).ToList();

                var groupedData = childCategories
                                       .Where(f => f.ParentId != 0)
                                       .Select(x => new SelectListItem
                                       {
                                           Value = x.Id.ToString(),
                                           Text = x.Text,
                                           Group = groupCategories.First(a => a.Name == parentCategories[x.ParentId.Value])
                                       }).ToList();


                //var data = BusinessCategories.Select(s => new
                //{
                //    Id = s.ParentId != null ? s.Id : -1,
                //    Name = s.ParentId != null ? " - "+ s.Name : "+ "+s.Name,
                //    ParentId = s.Id,
                //    OrderNumber = s.OrderNumber
                //}).OrderBy(o => o.ParentId).OrderBy(o => o.OrderNumber).ToList();


                ViewBag.BusinessCategoryId = groupedData;
                //data.Select(s => new SelectListItem()
                //{
                //    Value = Convert.ToString(s.Id),
                //    Text = s.Name
                //});

                var Memberships = await this.GetMemberships();
                ViewBag.MembershipId = Memberships.Select(s => new SelectListItem()
                {
                    Value = Convert.ToString(s.Id),
                    Text = s.Title
                });

                var Timezones = await this.GetTimeZone();
                ViewBag.TimezoneId = Timezones.Select(s => new SelectListItem()
                {
                    Value = Convert.ToString(s.Id),
                    Text = s.Title
                });

                return View(result);
            }
            catch(Exception ex)
            {
                result.HasError = true;
                result.Error = ex.Message.ToString();
                return View(result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register([Bind(Include ="Data")]ServiceDataViewModel<RegisterViewModel> model)
        {
            var result = new ResponseViewModel<string>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.BusinessService.Add(model.Data);
                if (response.Status)
                {
                    result.Status = true;
                    result.Message = response.Message;
                }
                else
                {
                    result.Status = false;
                    result.Message = response!=null ? response.Message : "There was a problem. Please try again later.";
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Contact()
        {
            return View();
        }


        private async Task<List<CountryViewModel>> GetCountries()
        {
            var response = await this.CountryService.Gets();
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<CountryViewModel>();
            }
        }

        private async Task<List<TimezoneViewModel>> GetTimeZone()
        {
            var response = await this.TimezoneService.Gets();
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<TimezoneViewModel>();
            }
        }

        private async Task<List<BusinessCategoryViewModel>> GetBusinessCategories()
        {
            var response = await this.BusinessCategoryService.Gets();
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessCategoryViewModel>();
            }
        }

        private async Task<List<MembershipViewModel>> GetMemberships()
        {
            var response = await this.MembershipService.Gets();
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<MembershipViewModel>();
            }
        }
    }
}