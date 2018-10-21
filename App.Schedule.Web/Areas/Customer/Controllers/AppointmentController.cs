using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Web.Models;
using App.Schedule.Domains.ViewModel;
using System.Collections.Generic;
using App.Schedule.Web.Areas.Customer.Controllers.Base;

namespace App.Schedule.Web.Areas.Customer.Controllers
{
    public class AppointmentController : AppointmentBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search, AppointmentViewStatus? type)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.CustomerId = RegisterCustomerViewModel.Customer.Id;
            ViewBag.BusinessId = RegisterCustomerViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterCustomerViewModel.Customer.ServiceLocationId;
            ViewBag.Type = type.HasValue ? (int)type.Value : 0;

            var result = await AppointmentService.Gets(RegisterCustomerViewModel.Customer.Id, TableType.CustomerId);
            if (result.Status)
            {
                if (type.HasValue)
                {
                    if (type.Value == AppointmentViewStatus.Completed)
                    {
                        result.Data = result.Data.Where(d => d.StatusType == (int)StatusType.Completed && d.IsActive == true).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Pending)
                    {
                        result.Data = result.Data.Where(d => d.StatusType != (int)StatusType.Canceled && d.StatusType != (int)StatusType.Completed && d.IsActive == true).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Deactivate)
                    {
                        result.Data = result.Data.Where(d => d.StatusType == (int)StatusType.Canceled || d.IsActive == false).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.CancelRequested)
                    {
                        result.Data = result.Data.Where(d => d.StatusType == (int)StatusType.CancelRequest && d.IsActive == true).ToList();
                    }
                    else
                    {
                        result.Data = result.Data.Where(d => d.StatusType != (int)StatusType.Completed && d.StatusType != (int)StatusType.Canceled && d.IsActive == true).ToList();
                    }
                }
                else
                {
                    result.Data = result.Data.Where(d => d.IsActive == true).ToList();
                }
                var data = result.Data.OrderByDescending(d => d.Id).ToList().OrderBy(d => d.StatusType).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Title.ToLower().Contains(search.ToLower()) && d.IsActive == true).ToList().ToPagedList(pageNumber, 10);
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
            try { 
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "customer" });

            var response = await this.AppointmentService.Get(id.Value);
            if (response.Status == false)
                return RedirectToAction("index", "appointment", new { area = "customer" });

            response.Data.ServiceLocationName = RegisterCustomerViewModel.ServiceLocation.Name;

            var BusinessOffers = await this.GetOffers();
            response.Data.BusinessOfferName = BusinessOffers.Find(d => d.Id == response.Data.BusinessOfferId).Name;

            var businessService = await this.GetBusinessServices(response.Data.BusinessServiceId.Value);
            response.Data.BusinessServiceName = string.Format("{0} (${1})", businessService.Name, Math.Round(businessService.Cost.Value, 2));

            var employees = await this.GetBusinessEmployee(id);
            ViewBag.BusinessEmployeeId = employees;
            //var invitees = await this.AppointmentInviteeService.Gets(response.Data.Id, TableType.AppointmentInvitee);

            //if (invitees.Status)
            //{
            //    var employees = await this.GetBusinessEmployee(id);
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
            catch (Exception ex)
            {
                return RedirectToAction("index","dashboard", new { area = "customer" });
            }
        }

        [NonAction]
        private async Task<List<ServiceLocationViewModel>> GetServiceLocations()
        {
            var response = await this.ServiceLocationService.Gets(RegisterCustomerViewModel.Business.Id, TableType.BusinessId);
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
            var response = await this.BusinessCustomerService.Gets(RegisterCustomerViewModel.Business.Id, TableType.BusinessId);
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
            var response = await this.BusinessOfferService.Gets(RegisterCustomerViewModel.Business.Id, TableType.BusinessId);
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
                var response = await this.BusinessEmployeeService.Gets(RegisterCustomerViewModel.Customer.Id, TableType.BusinessId);
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
                return RedirectToAction("index", "appointment", new { area = "customer" });

            var pageNumber = page ?? 1;
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentDocumentViewModel>>();
            ViewBag.AppointmentId = id.Value;
            var response = await this.AppointmentDocumentService.Gets(id.Value,TableType.AppointmentDocument);
            if (response.Status)
            {
                model.Data = response.Data.OrderByDescending(d => d.Id).ToPagedList(pageNumber, 10);
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
                return RedirectToAction("index", "appointment", new { area = "customer" });

            var pageNumber = page ?? 1;
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentFeedbackViewModel>>();

            var response = await this.AppointmentFeedbackService.Gets(id.Value);
            if (response.Status)
            {
                model.Data = response.Data.OrderByDescending(d => d.Id).ToPagedList(pageNumber, 10);
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
            var ratingValue = new List<object>();
            ratingValue.Add(new { Value = 1, Text = "1 Star" });
            ratingValue.Add(new { Value = 2, Text = "2 Star" });
            ratingValue.Add(new { Value = 3, Text = "3 Star" });
            ratingValue.Add(new { Value = 4, Text = "4 Star" });
            ratingValue.Add(new { Value = 5, Text = "5 Star" });
            ViewBag.Rating = new SelectList(ratingValue, "Value", "Text");

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
                model.Data.IsEmployee = false;
                model.Data.BusinessCustomerId = RegisterCustomerViewModel.Customer.Id;
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

        [HttpGet]
        public async Task<ActionResult> Cancel(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "customer" });

            var response = await this.AppointmentService.Get(id.Value);
            if (response.Status == false)
                return RedirectToAction("index", "appointment", new { area = "customer" });

            response.Data.ServiceLocationName = RegisterCustomerViewModel.ServiceLocation.Name;

            var BusinessOffers = await this.GetOffers();
            response.Data.BusinessOfferName = BusinessOffers.Find(d => d.Id == response.Data.BusinessOfferId).Name;

            var businessService = await this.GetBusinessServices(response.Data.BusinessServiceId.Value);
            response.Data.BusinessServiceName = string.Format("{0} (${1})", businessService.Name, Math.Round(businessService.Cost.Value, 2));

            var employees = await this.GetBusinessEmployee(id);
            ViewBag.BusinessEmployeeId = employees;

            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;

            return View(response);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cancel([Bind(Include = "Data")]ResponseViewModel<AppointmentViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentFeedbackViewModel>();

            if (String.IsNullOrEmpty(model.Data.CancelReason))
            {
                result.Message = "Please provide cancelation reason.";
            }
            else
            {
                model.Data.StatusType = (int)StatusType.CancelRequest;
                model.Data.BusinessCustomerId = RegisterCustomerViewModel.Customer.Id;
                var response = await this.AppointmentService.Cancel(model.Data.Id, StatusType.CancelRequest, model.Data.CancelReason);
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

        [HttpGet]
        public async Task<ActionResult> InvoiceHtml(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var response = await this.AppointmentService.GetPaymentById(id.Value);
            response.Status = true;

            if (response == null || response.Data == null)
                return RedirectToAction("payments", "appointment", new { area = "admin" });

            return View(response);
        }
    }
}