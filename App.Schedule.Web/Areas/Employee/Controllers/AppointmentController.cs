using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Web.Models;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Employee.Controllers.Base;

namespace App.Schedule.Web.Areas.Employee.Controllers
{
    public class AppointmentController : AppointmentBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search, AppointmentViewStatus? type)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.EmployeeId = RegisterViewModel.Employee.Id;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.ServiceLocation.Id;

            var result = await AppointmentService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            if (result.Status)
            {
                if (type.HasValue)
                {
                    if (type.Value == AppointmentViewStatus.Canceled)
                    {
                        result.Data = result.Data.Where(d => d.StatusType == (int)StatusType.Canceled).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Completed)
                    {
                        result.Data = result.Data.Where(d => d.StatusType == (int)StatusType.Completed).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Pending)
                    {
                        result.Data = result.Data.Where(d => d.StatusType != (int)StatusType.Canceled && d.StatusType != (int)StatusType.Completed).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Deactivate)
                    {
                        result.Data = result.Data.Where(d => d.IsActive == false).ToList();
                    }
                }
                var data = result.Data.OrderByDescending(d => d.Id).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 5);
                }
                else
                {
                    model.Data = data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> View(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "employee" });

            var response = await this.AppointmentService.Get(id.Value);
            if (response.Status == false)
                return RedirectToAction("index", "appointment", new { area = "employee" });

            response.Data.ServiceLocationName = RegisterViewModel.ServiceLocation.Name;

            var BusinessOffers = await this.GetOffers();
            response.Data.BusinessOfferName = BusinessOffers.Find(d => d.Id == response.Data.BusinessOfferId).Name;

            var businessService = await this.GetBusinessServices(response.Data.BusinessServiceId.Value);
            response.Data.BusinessServiceName = string.Format("{0} (${1})", businessService.Name, Math.Round(businessService.Cost.Value, 2));

            var employees = await this.GetBusinessEmployee(id);
            ViewBag.BusinessEmployeeId = employees;
            //var invitees = await this.AppointmentInviteeService.Gets(response.Data.Id, TableType.AppointmentInvitee);

            //if (invitees.Status)
            //{
            //    var employees = await this.GetBusinessEmployee();
            //    var appointmentInvitees = new List<BusinessEmployeeViewModel>();
            //    var employee = employees.Find(d => d.Id == response.Data.BusinessEmployeeId);
            //    if (employee != null)
            //        appointmentInvitees.Add(employee);

            //    foreach (var invitee in invitees.Data)
            //    {
            //        employee = employees.Find(d => d.Id == invitee.BusinessEmployeeId);
            //        if (employee != null)
            //            appointmentInvitees.Add(employee);
            //    }
            //    ViewBag.BusinessEmployeeId = appointmentInvitees;
            //}


            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;

            return View(response);
        }


        [NonAction]
        private async Task<List<ServiceLocationViewModel>> GetServiceLocations()
        {
            var response = await this.ServiceLocationService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<ServiceLocationViewModel>();
            }
        }

        [NonAction]
        private async Task<List<BusinessCustomerViewModel>> GetCustomers()
        {
            var response = await this.BusinessCustomerService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessCustomerViewModel>();
            }
        }

        [NonAction]
        private async Task<List<BusinessOfferViewModel>> GetOffers()
        {
            var response = await this.BusinessOfferService.Gets(-1, TableType.All);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessOfferViewModel>();
            }
        }

        [NonAction]
        private async Task<BusinessServiceViewModel> GetBusinessServices(long serviceId)
        {
            var response = await this.BusinessService.Get(serviceId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new BusinessServiceViewModel();
            }
        }
        [NonAction]
        private async Task<List<BusinessEmployeeViewModel>> GetBusinessEmployee(long? appointmentId)
        {
            var employees = new List<BusinessEmployeeViewModel>(); ;
            if (appointmentId.HasValue)
            {
                var response = await this.BusinessEmployeeService.Gets(appointmentId, TableType.AppointmentInvitee);
                if (response != null)
                    employees = response.Data;
            }
            else
            {
                var response = await this.BusinessEmployeeService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
                if (response != null)
                    employees = response.Data;
            }
            return employees;
        }

        [NonAction]
        private async Task<List<SelectListItem>> GetGroupedDocumentCategories()
        {
            var DocumentCategories = await this.GetDocumentCategories();
            var parentCategories = DocumentCategories.ToDictionary(d => d.Id, d => d.Name);
            var groupCategories = DocumentCategories.Select(s => s.Name).Select(ss => new SelectListGroup() { Name = ss }).ToList();

            var childCategories = (from c in DocumentCategories
                                   join p in DocumentCategories
                                   on c.ParentId equals p.Id
                                   select new
                                   {
                                       Id = c.Id,
                                       Text = c.Name,
                                       ParentId = c.ParentId.Value
                                   }).ToList();

            var groupDocumentCategories = childCategories
                                   .Where(f => f.ParentId != 0)
                                   .Select(x => new SelectListItem
                                   {
                                       Value = x.Id.ToString(),
                                       Text = x.Text,
                                       Group = groupCategories.First(a => a.Name == parentCategories[x.ParentId])
                                   }).ToList();

            return groupDocumentCategories;
        }

        [NonAction]
        private async Task<List<DocumentCategoryViewModel>> GetDocumentCategories()
        {
            var response = await this.DocumentCategoryService.Gets();
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<DocumentCategoryViewModel>();
            }
        }

        [HttpGet]
        public async Task<ActionResult> Attachments(long? id, int? page)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "employee" });

            var pageNumber = page ?? 1;
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentDocumentViewModel>>();
            ViewBag.AppointmentId = id.Value;
            var response = await this.AppointmentDocumentService.Gets(id.Value);
            if (response.Status)
            {
                model.Data = response.Data.OrderByDescending(d => d.Id).ToPagedList(pageNumber, 5);
                model.Status = response.Status;
                model.Message = response.Message;
            }
            else
            {
                model.Status = false;
                model.Message = response.Message;
            }
            ViewBag.AppointmentId = id.Value;
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Feedbacks(long? id, int? page)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "employee" });

            var pageNumber = page ?? 1;
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentFeedbackViewModel>>();

            var response = await this.AppointmentFeedbackService.Gets(id.Value);
            if (response.Status)
            {
                model.Data = response.Data.OrderByDescending(d => d.Id).ToPagedList(pageNumber, 5);
                model.Status = response.Status;
                model.Message = response.Message;
            }
            else
            {
                model.Status = false;
                model.Message = response.Message;
            }
            ViewBag.AppointmentId = id.Value;
            return View(model);
        }

        [HttpGet]
        public ActionResult Add_Feedback(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");

            var response = this.ResponseHelper.GetResponse<AppointmentFeedbackViewModel>();
            response.Data = new AppointmentFeedbackViewModel();
            response.Status = true;
            response.Data.AppointmentId = id.Value;
            return View(response);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add_Feedback([Bind(Include = "Data")]ResponseViewModel<AppointmentFeedbackViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentFeedbackViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                model.Data.IsEmployee = true;
                model.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
                model.Data.Rating = 0;
                var response = await this.AppointmentFeedbackService.Add(model.Data);
                if (response == null)
                {
                    result.Status = false;
                    result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                }
                else
                {
                    result.Status = response.Status;
                    result.Message = response.Message;
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

    }
}