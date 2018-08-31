using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Admin.Controllers
{
    public class BusinessController : BusinessBaseController
    {
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = new ServiceDataViewModel<IPagedList<BusinessViewModel>>();
            try
            {
                Session["HomeLink"] = "Business";
                var pageNumber = page ?? 1;
                ViewBag.search = search;

                var response = await this.BusinessService.Gets();
                if (response.Status)
                {
                    var data = response.Data;
                    if (search == null)
                    {
                        model.Data = data.ToPagedList<BusinessViewModel>(pageNumber, 10);
                        return View(model);
                    }
                    else
                    {
                        model.Data = data.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
                        return View(model);
                    }
                }
                else
                {
                    model.HasError = !response.Status;
                    model.Error = response.Message;
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        public async Task<ActionResult> Employees(long? id, int? page, string search)
        {
            var model = new ServiceDataViewModel<IPagedList<BusinessEmployeeViewModel>>();
            try
            {
                if (!id.HasValue)
                    return View(model);

                Session["HomeLink"] = "BusinessEmployee";
                var pageNumber = page ?? 1;
                ViewBag.search = search;

                var response = await this.BusinessService.GetEmployees(id.Value,TableType.BusinessId);
                if (response.Status)
                {
                    var data = response.Data;
                    if (search == null)
                    {
                        model.Data = data.ToPagedList<BusinessEmployeeViewModel>(pageNumber, 10);
                        return View(model);
                    }
                    else
                    {
                        model.Data = data.Where(d => d.FirstName.ToLower().Contains(search.ToLower()) || d.LastName.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
                        return View(model);
                    }
                }
                else
                {
                    model.HasError = !response.Status;
                    model.Error = response.Message;
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        public async Task<ActionResult> Customers(long? id, int? page, string search)
        {
            var model = new ServiceDataViewModel<IPagedList<BusinessCustomerViewModel>>();
            try
            {
                Session["HomeLink"] = "BusinessCustomers";
                var pageNumber = page ?? 1;
                ViewBag.search = search;

                var response = await this.BusinessService.GetCustomers(id.Value, TableType.BusinessId);
                if (response.Status)
                {
                    var data = response.Data;
                    if (search == null)
                    {
                        model.Data = data.ToPagedList<BusinessCustomerViewModel>(pageNumber, 10);
                        return View(model);
                    }
                    else
                    {
                        model.Data = data.Where(d => d.FirstName.ToLower().Contains(search.ToLower()) || d.LastName.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
                        return View(model);
                    }
                }
                else
                {
                    model.HasError = !response.Status;
                    model.Error = response.Message;
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        public async Task<ActionResult> Appointments(long? id,string user, int? page, string search)
        {
            var model = new ServiceDataViewModel<IPagedList<AppointmentViewModel>>();
            try
            {
                Session["HomeLink"] = string.Format("appointments?id={0}&user={1}",id.Value,user);
                var pageNumber = page ?? 1;
                ViewBag.search = search;
                var type = user.ToLower() == "cust" ? TableType.CustomerId : TableType.EmployeeId;
                var response = await this.BusinessService.GetAppointmentss(id.Value, type);
                if (response.Status)
                {
                    var data = response.Data;
                    if (search == null)
                    {
                        model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                        return View(model);
                    }
                    else
                    {
                        model.Data = data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
                        return View(model);
                    }
                }
                else
                {
                    model.HasError = !response.Status;
                    model.Error = response.Message;
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }
    }
}