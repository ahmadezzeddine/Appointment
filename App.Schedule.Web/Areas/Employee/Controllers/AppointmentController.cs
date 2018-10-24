using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Web.Models;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Helpers;
using App.Schedule.Web.Areas.Employee.Controllers.Base;
using App.Schedule.Domains.Helpers;

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
            model.hasAdd = true;
            ViewBag.type = type.HasValue ? type.Value : 0;
            ViewBag.EmployeeId = RegisterViewModel.Employee.Id;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.ServiceLocation.Id;

            var result = await AppointmentService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;

            if (result.Status)
            {
                var appointmentsModel = result.Data.Where(d => d.BusinessEmployeeId != null).ToList();
                model.hasAdd = appointmentsModel != null && appointmentsModel.Count <= total ? true : false;
                if (type.HasValue)
                {
                    if (type.Value == AppointmentViewStatus.Completed)
                    {
                        result.Data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.Completed && d.IsActive == true).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Pending)
                    {
                        result.Data = appointmentsModel.Where(d => d.StatusType != (int)StatusType.Canceled && d.StatusType != (int)StatusType.Completed && d.IsActive == true).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.Deactivate)
                    {
                        result.Data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.Canceled || d.IsActive == false).ToList();
                    }
                    else if (type.Value == AppointmentViewStatus.CancelRequested)
                    {
                        result.Data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.CancelRequest).ToList();
                    }
                    else
                    {
                        result.Data = result.Data.Where(d => d.StatusType != (int)StatusType.Canceled && d.StatusType != (int)StatusType.CancelRequest && d.IsActive == true).ToList();
                    }
                }
                else
                {
                    result.Data = result.Data.Where(d => d.StatusType != (int)StatusType.Canceled && d.StatusType != (int)StatusType.CancelRequest && d.IsActive == true).ToList();
                    //result.Data = appointmentsModel.Where(d => d.StatusType != (int)StatusType.Completed && d.StatusType != (int)StatusType.Canceled && d.IsActive == true).ToList();
                }
                var data = result.Data.OrderByDescending(d => d.Id).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Title.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
        public async Task<ActionResult> Payment(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            ViewBag.EmployeeId = RegisterViewModel.Employee.Id;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.ServiceLocation.Id;

            var result = await AppointmentService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;

            if (result.Status)
            {
                var appointmentsModel = result.Data.Where(d => d.BusinessEmployeeId != null && d.BusinessEmployeeId == RegisterViewModel.Employee.Id).ToList();
                model.hasAdd = appointmentsModel != null && appointmentsModel.Count <= total ? true : false;
                var data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.Completed && d.IsActive == true).OrderByDescending(d => d.Id).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Title.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
            try
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
                response.Data.StartDate = response.Data.StartDate.Value.UtcToLocal();
                response.Data.EndDate = response.Data.StartDate.Value.UtcToLocal();
                return View(response);
            }
            catch(Exception ex)
            {
                return RedirectToAction("index","dashboard", new { area = "employee" });
            }
        }

        public async Task<ActionResult> GetCustomerById(long? id)
        {
            if (id.HasValue)
            {
                var response = await this.BusinessCustomerService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
                if (response.Status)
                {
                    var customer = response.Data.Find(d => d.Id == id.Value);
                    return Json(new { status = true, data = customer, message = "found" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { status = false, data = "", message = "No records found" }, JsonRequestBehavior.AllowGet);
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
            var response = await this.BusinessCustomerService.Gets(RegisterViewModel.Employee.ServiceLocationId, TableType.ServiceLocationId);
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
            var response = await this.BusinessOfferService.Gets(RegisterViewModel.Employee.ServiceLocationId, TableType.ServiceLocationId);
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
            var response = await this.AppointmentDocumentService.Gets(id.Value, TableType.AppointmentDocument);
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
        [ActionName("add_document")]
        public async Task<ActionResult> Add_Document(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");

            var response = this.ResponseHelper.GetResponse<AppointmentDocumentViewModel>();
            response.Data = new AppointmentDocumentViewModel();
            response.Status = true;

            ViewBag.DocumentCategoryId = await this.GetGroupedDocumentCategories();

            //var documenttype = from DocumentType e in Enum.GetValues(typeof(DocumentType))
            //                   select new
            //                   {
            //                       Value = (int)e,
            //                       Text = e.ToString()
            //                   };
            //ViewBag.DocumentType = new SelectList(documenttype, "Value", "Text");

            response.Data.AppointmentId = id.Value;
            return View(response);
        }

        [ValidateAntiForgeryToken]
        [ActionName("add_document")]
        public async Task<ActionResult> Add_Document([Bind(Include = "Data")]ResponseViewModel<AppointmentDocumentViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentDocumentViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                model.Data.IsEmployeeUpload = true;
                var response = await this.AppointmentDocumentService.Add(model.Data);
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
        public async Task<ActionResult> Feedbacks(long? id, int? page,string search)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "employee" });

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
            if (search != null)
            {
                model.Data = model.Data.Where(d => d.Feedback.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
            }
            else
            {
                model.Data = model.Data.ToPagedList(pageNumber, 10);
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
                model.Data.IsEmployee = true;
                model.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
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

        public async Task<ActionResult> DocumentUpload()
        {
            var httpRequest = HttpContext.Request;
            if (httpRequest.Files.Count > 0)
            {
                var response = await this.AppointmentService.FileUpload(httpRequest);
                if (response == null)
                {
                    response.Status = false;
                    response.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                }
                else
                {
                    response.Data = response.Data;
                    response.Status = response.Status;
                    response.Message = response.Message;
                }
                return Json(new { status = response.Status, data = response.Data, message = response.Message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = false, data = "", message = "No file selected." }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Add()
        {
            var appointmentViewModel = new AppointmentViewModel();
            var response = this.ResponseHelper.GetResponse<AppointmentViewModel>();
            response.Data = new AppointmentViewModel();
            response.Status = true;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

            var ServiceLocations = await this.GetServiceLocations();
            ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = s.Name
            });

            var BusinessCustomers = await this.GetCustomers();
            ViewBag.BusinessCustomerId = BusinessCustomers.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0} {1} - {2}", s.FirstName, s.LastName, s.City)
            });

            var BusinessOffers = await this.GetOffers();
            ViewBag.BusinessOfferId = BusinessOffers.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0} {1} - valid: {2}", s.Code, s.Name, s.ValidTo)
            });
            var BusinessServices = await this.GetBusinessServices();
            ViewBag.BusinessServiceId = BusinessServices.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0:C2} {1}", s.Cost, s.Name)
            });

            var hourHelper = new BusinessHourHelper(this.Token, this.RegisterViewModel.Employee.ServiceLocationId.Value);

            var fromHours = await hourHelper.GetHoursOfDay((int)DateTime.Now.DayOfWeek);
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value
            });
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value
            });

            var patternType = from PatternTypeOnce e in Enum.GetValues(typeof(PatternTypeOnce))
                              select new
                              {
                                  ID = (int)e,
                                  Name = e.ToString()
                              };
            ViewBag.PatternType = new SelectList(patternType, "Id", "Name");
            response.Data.StartDate = DateTime.Now.Date;
            response.Data.EndDate = DateTime.Now.Date;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            response.Data.IsActive = true;
            return View(response);
        }

        [HttpPost]
        [ActionName("make")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "Data")]ResponseViewModel<AppointmentViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                if (!model.Data.IsAllDayEvent)
                {
                    if (model.Data.StartTime >= model.Data.EndTime)
                    {
                        result.Status = false;
                        result.Message = "Please provide a valid end time.";
                        return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (model.Data.StartDate >= model.Data.EndDate)
                {
                    result.Status = false;
                    result.Message = "Please provide a valid end date.";
                    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                }
                if (model.Data.BusinessCustomerId <= 0)
                {
                    result.Status = false;
                    result.Message = "Please select a customer.";
                    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                }

                model.Data.SelectedEmployeeIds = new List<long>() { RegisterViewModel.Employee.Id };
                model.Data.StatusType = (int)StatusType.Confirmed;
                var startdate = model.Data.StartDate.HasValue ? model.Data.StartDate.Value : DateTime.Now;
                var starttime = model.Data.StartTime.HasValue ? model.Data.StartTime.Value : DateTime.Now;
                //var enddate = model.Data.EndDate.HasValue ? model.Data.EndDate.Value : DateTime.Now;
                var enddate = model.Data.StartDate.HasValue ? model.Data.StartDate.Value : DateTime.Now;
                var endtime = model.Data.EndTime.HasValue ? model.Data.EndTime.Value : DateTime.Now;
                model.Data.StartTime = new DateTime(startdate.Year, startdate.Month, startdate.Day, starttime.Hour, starttime.Minute, starttime.Second);
                model.Data.EndTime = new DateTime(enddate.Year, enddate.Month, enddate.Day, endtime.Hour, endtime.Minute, endtime.Second);
                var response = await this.AppointmentService.Add(model.Data);
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
        public async Task<ActionResult> Edit(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var response = await this.AppointmentService.Get(id.Value);
            response.Status = true;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            //response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}{3}", Unique.GetValue(), DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);
            response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

            var ServiceLocations = await this.GetServiceLocations();
            ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = s.Name
            });

            var BusinessCustomers = await this.GetCustomers();
            ViewBag.BusinessCustomerId = BusinessCustomers.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0} {1} - {2}", s.FirstName, s.LastName, s.City)
            });

            var BusinessOffers = await this.GetOffers();
            ViewBag.BusinessOfferId = BusinessOffers.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0} {1} - valid: {2}", s.Code, s.Name, s.ValidTo)
            });
            var BusinessServices = await this.GetBusinessServices();
            ViewBag.BusinessServiceId = BusinessServices.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0:C2} {1}", s.Cost, s.Name)
            });

            //Hours
            var hourHelper = new BusinessHourHelper(this.Token, response.Data.ServiceLocationId.Value);

            var fromHours = await hourHelper.GetHoursOfDay((int)response.Data.StartDate.Value.UtcToLocal().DayOfWeek);
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = (response.Data.StartTime.HasValue && s.Value.Contains(response.Data.StartTime.Value.UtcToLocal().ToString("hh:mm tt"))) ? true : false
            });
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = (response.Data.EndTime.HasValue && s.Value.Contains(response.Data.EndTime.Value.UtcToLocal().ToString("hh:mm tt"))) ? true : false
            });
            response.Data.StartDate = response.Data.StartDate.Value.UtcToLocal();
            response.Data.EndDate = response.Data.StartDate.Value.UtcToLocal();
            //End

            //Pattern type
            var patternType = from PatternTypeOnce e in Enum.GetValues(typeof(PatternTypeOnce))
                              select new
                              {
                                  ID = (int)e,
                                  Name = e.ToString()
                              };
            ViewBag.PatternType = new SelectList(patternType, "Id", "Name");
            //End

            var employees = await this.GetBusinessEmployee(RegisterViewModel.Business.Id, TableType.BusinessId);
            ViewBag.BusinessEmployeeId = employees;

            return View(response);
        }

        [HttpPost]
        [ActionName("update")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")] ResponseViewModel<AppointmentViewModel> model)
        {
            var result = new ResponseViewModel<BusinessHolidayViewModel>();
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
                    if (!model.Data.IsAllDayEvent)
                    {
                        if (model.Data.StartTime >= model.Data.EndTime)
                        {
                            result.Status = false;
                            result.Message = "Please provide a valid end time.";
                            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (model.Data.StartDate >= model.Data.EndDate)
                    {
                        result.Status = false;
                        result.Message = "Please provide a valid end date.";
                        return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    }
                    if (model.Data.BusinessCustomerId <= 0)
                    {
                        result.Status = false;
                        result.Message = "Please select a customer.";
                        return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    }
                    model.Data.SelectedEmployeeIds = new List<long>() { model.Data.BusinessEmployeeId.Value };
                    //if (model.Data.SelectedEmployeeIds == null || model.Data.SelectedEmployeeIds.Count <= 0)
                    //{
                    //    result.Status = false;
                    //    result.Message = "Please select at least one invitee.";
                    //    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    //}
                    model.Data.StatusType = (int)StatusType.Resheduled;

                    var startdate = model.Data.StartDate.HasValue ? model.Data.StartDate.Value : DateTime.Now;
                    var starttime = model.Data.StartTime.HasValue ? model.Data.StartTime.Value : DateTime.Now;
                    var enddate = model.Data.EndDate.HasValue ? model.Data.EndDate.Value : DateTime.Now;
                    var endtime = model.Data.EndTime.HasValue ? model.Data.EndTime.Value : DateTime.Now;
                    model.Data.StartTime = new DateTime(startdate.Year, startdate.Month, startdate.Day, starttime.Hour, starttime.Minute, starttime.Second);
                    model.Data.EndTime = new DateTime(enddate.Year, enddate.Month, enddate.Day, endtime.Hour, endtime.Minute, endtime.Second);

                    var response = await this.AppointmentService.Update(model.Data);
                    if (response.Status)
                    {
                        result.Status = true;
                        result.Message = response.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = response.Message;
                    }

                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private async Task<List<BusinessEmployeeViewModel>> GetBusinessEmployee(long? appointmentId, TableType type)
        {
            var employees = new List<BusinessEmployeeViewModel>(); ;
            var response = await this.BusinessEmployeeService.Gets(appointmentId, type);
            if (response != null)
                employees = response.Data;
            return employees;
        }

        [NonAction]
        private async Task<List<BusinessServiceViewModel>> GetBusinessServices()
        {
            var response = await this.BusinessService.Gets(RegisterViewModel.Business.Id, (int)TableType.BusinessId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessServiceViewModel>();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetHours(DateTime date, long? LocationId)
        {
            if (!LocationId.HasValue)
                LocationId = RegisterViewModel.Employee.ServiceLocationId;
            var hourHelper = new BusinessHourHelper(this.Token, LocationId.Value);
            var getHours = await hourHelper.GetHoursOfDay((int)date.DayOfWeek);
            var hours = getHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value
            });
            return Json(hours, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Deactive(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var response = await this.AppointmentService.Get(id.Value);
            response.Status = true;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

            var ServiceLocations = await this.GetServiceLocations();
            ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = s.Name
            });

            var BusinessCustomers = await this.GetCustomers();
            ViewBag.BusinessCustomerId = BusinessCustomers.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0} {1} [Ciity - {2}]", s.FirstName, s.LastName, s.City)
            });

            var BusinessOffers = await this.GetOffers();
            ViewBag.BusinessOfferId = BusinessOffers.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0} {1} - valid: {2}", s.Code, s.Name, s.ValidTo)
            });
            var BusinessServices = await this.GetBusinessServices();
            ViewBag.BusinessServiceId = BusinessServices.Select(s => new SelectListItem()
            {
                Value = Convert.ToString(s.Id),
                Text = String.Format("{0:C2} {1}", s.Cost, s.Name)
            });

            //Hours
            var fromHours = Hour.GetHoursOfDay();
            var getFromHour = response.Data.StartDate.Value.UtcToLocal().ToShortTimeString();
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getFromHour ? true : false
            });
            var getToHour = response.Data.EndDate.Value.UtcToLocal().ToShortTimeString();
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getToHour ? true : false
            });
            response.Data.StartDate = response.Data.StartDate.Value.UtcToLocal();
            response.Data.EndDate = response.Data.StartDate.Value.UtcToLocal();
            //End

            //Pattern type
            var patternType = from PatternType e in Enum.GetValues(typeof(PatternType))
                              select new
                              {
                                  ID = (int)e,
                                  Name = e.ToString()
                              };
            ViewBag.PatternType = new SelectList(patternType, "Id", "Name");
            //End

            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactive([Bind(Include = "Data")] ResponseViewModel<AppointmentViewModel> model)
        {
            var result = new ResponseViewModel<BusinessHolidayViewModel>();
            try
            {
                if (model.Data.StatusType == null) model.Data.StatusType = 0;
                var response = await this.AppointmentService.Deactive(model.Data.Id, model.Data.IsActive);
                if (response.Status)
                {
                    result.Status = true;
                    result.Message = response.Message;
                }
                else
                {
                    result.Status = false;
                    result.Message = response.Message;
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Close(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var response = await this.AppointmentService.GetPayment(id.Value);
            response.Status = true;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

            var billingType = from BillingType e in Enum.GetValues(typeof(BillingType))
                              select new
                              {
                                  Value = (int)e,
                                  Text = e.ToString()
                              };
            ViewBag.BillingType = new SelectList(billingType, "Value", "Text");

            var cardType = from CardType e in Enum.GetValues(typeof(CardType))
                           select new
                           {
                               Value = (int)e,
                               Text = e.ToString()
                           };
            ViewBag.CardType = new SelectList(cardType, "Value", "Text");

            response.Data.StartDate = response.Data.StartDate.Value.UtcToLocal();
            response.Data.EndDate = response.Data.StartDate.Value.UtcToLocal();
            response.Data.Payment.IsPaid = true;
            //End

            return View(response);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Close([Bind(Include = "Data")] ResponseViewModel<AppointmentPayViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentPayViewModel>();
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
                    result.Status = false;
                    model.Data.Payment.IsPaid = true;
                    if (model.Data.StatusType == null) model.Data.StatusType = (int)StatusType.Completed;
                    model.Data.Payment.AppointmentId = model.Data.Id;
                    if (model.Data.Payment.BillingType == (int)BillingType.Cheque && model.Data.Payment.ChequeNumber == null)
                    {
                        result.Message = "Please enter cheque number.";
                        return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    }
                    else if (model.Data.Payment.BillingType == (int)BillingType.CreditCard)
                    {
                        if (model.Data.Payment.CCardNumber == null)
                        {
                            result.Message = "Please enter Card Number.";
                            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.Data.Payment.CCFirstName == null)
                        {
                            result.Message = "Please enter First Name.";
                            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                        }
                        else if (model.Data.Payment.CCLastName == null)
                        {
                            result.Message = "Please enter Last Name.";
                            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    var response = await this.AppointmentService.Resheduled(model.Data, "");
                    if (response.Status)
                    {
                        result.Status = true;
                        result.Message = response.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = response.Message;
                    }

                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
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