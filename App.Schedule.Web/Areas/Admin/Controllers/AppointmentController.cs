using App.Schedule.Domains.ViewModel;
using PagedList;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.Helpers;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class AppointmentController : AppointmentBaseController
    {
        AppointmentViewModel appointmentViewModel;

        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (result.Status)
            {
                var data = result.Data.Where(d => d.StatusType != (int)StatusType.Completed).OrderByDescending(d => d.Id).ToList();
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
        public async Task<ActionResult> Add()
        {
            appointmentViewModel = new AppointmentViewModel();
            var response = this.ResponseHelper.GetResponse<AppointmentViewModel>();
            response.Data = new AppointmentViewModel();
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

            var fromHours = Hour.GetHoursOfDay();
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

            var patternType = from PatternType e in Enum.GetValues(typeof(PatternType))
                              select new
                              {
                                  ID = (int)e,
                                  Name = e.ToString()
                              };
            ViewBag.PatternType = new SelectList(patternType, "Id", "Name");
            response.Data.StartDate = DateTime.Now.Date;
            response.Data.EndDate = DateTime.Now.Date;

            var employees = await this.GetBusinessEmployee();
            var currentEmployee = employees.Find(d => d.Id == RegisterViewModel.Employee.Id);
            employees.Remove(currentEmployee);
            ViewBag.BusinessEmployeeId = employees;

            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("make")]
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
                    if (model.Data.StartTime > model.Data.EndTime)
                    {
                        result.Status = false;
                        result.Message = "Provide valid end time.";
                        return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (model.Data.StartDate > model.Data.EndDate)
                {
                    result.Status = false;
                    result.Message = "Provide valid end date.";
                    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                }
                model.Data.StatusType = (int)StatusType.Confirmed;
                var startdate = model.Data.StartDate.HasValue ? model.Data.StartDate.Value : DateTime.Now;
                var starttime = model.Data.StartTime.HasValue ? model.Data.StartTime.Value : DateTime.Now;
                var enddate = model.Data.EndDate.HasValue ? model.Data.EndDate.Value : DateTime.Now;
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
            var fromHours = Hour.GetHoursOfDay();
            var getFromHour = response.Data.StartDate.Value.ToShortTimeString();
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getFromHour ? true : false
            });
            var getToHour = response.Data.EndDate.Value.ToShortTimeString();
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getToHour ? true : false
            });
            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;
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

            var employees = await this.GetBusinessEmployee();
            var currentEmployee = employees.Find(d => d.Id == RegisterViewModel.Employee.Id);
            employees.Remove(currentEmployee);
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
                            result.Message = "Provide valid end time.";
                            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (model.Data.StartDate > model.Data.EndDate)
                    {
                        result.Status = false;
                        result.Message = "Provide valid end date.";
                        return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                    }
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

        [HttpGet]
        public async Task<ActionResult> Deactive(long? id)
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
            var getFromHour = response.Data.StartDate.Value.ToShortTimeString();
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getFromHour ? true : false
            });
            var getToHour = response.Data.EndDate.Value.ToShortTimeString();
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getToHour ? true : false
            });
            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;
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

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactive([Bind(Include = "Data")] ResponseViewModel<AppointmentViewModel> model)
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
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(long? id)
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
            var getFromHour = response.Data.StartDate.Value.ToShortTimeString();
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getFromHour ? true : false
            });
            var getToHour = response.Data.EndDate.Value.ToShortTimeString();
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = s.Value == getToHour ? true : false
            });
            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;
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

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")] ResponseViewModel<AppointmentViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var response = await this.AppointmentService.Delete(model.Data.Id);
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
        public async Task<ActionResult> Search(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (result.Status)
            {
                var data = result.Data;
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 5);
                }
                else
                {
                    model.Data = data.Where(d => d.GlobalAppointmentId.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 5);
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
        public async Task<ActionResult> Payments(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (result.Status)
            {
                var data = result.Data.Where(d => d.StatusType == (int)StatusType.Completed).OrderByDescending(d => d.Id).ToList();
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
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var response = await this.AppointmentService.Get(id.Value);
            response.Status = true;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

            var ServiceLocations = await this.GetServiceLocations();
            response.Data.ServiceLocationName = ServiceLocations.Find(d => d.Id == response.Data.ServiceLocationId).Name;

            var BusinessOffers = await this.GetOffers();
            response.Data.BusinessOfferName = BusinessOffers.Find(d => d.Id == response.Data.BusinessOfferId).Name;

            var BusinessServices = await this.GetBusinessServices();
            response.Data.BusinessServiceName = BusinessServices.Find(d => d.Id == response.Data.BusinessServiceId).Name;

            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;

            var employees = await this.GetBusinessEmployee();
            var currentEmployee = employees.Find(d => d.Id == RegisterViewModel.Employee.Id);
            employees.Remove(currentEmployee);
            ViewBag.BusinessEmployeeId = employees;

            return View(response);
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

            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;
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
                    if (model.Data.StatusType == null) model.Data.StatusType = (int)StatusType.Completed;
                    model.Data.Payment.AppointmentId = model.Data.Id;
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
        public async Task<ActionResult> Attachments(long? id, int? page)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var pageNumber = page ?? 1;
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentDocumentViewModel>>();

            var response = await this.AppointmentDocumentService.Gets(id.Value);
            if (response.Status)
            {
                model.Data = response.Data.OrderByDescending(d => d.Id).ToPagedList(pageNumber,5);
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

            var documenttype = from DocumentType e in Enum.GetValues(typeof(DocumentType))
                              select new
                              {
                                  Value = (int)e,
                                  Text = e.ToString()
                              };
            ViewBag.DocumentType = new SelectList(documenttype, "Value", "Text");

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
        public async Task<ActionResult> Delete_Document(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");

            var response = await this.AppointmentDocumentService.Get(id.Value);
            ViewBag.DocumentCategoryId = await this.GetGroupedDocumentCategories();

            var documenttype = from DocumentType e in Enum.GetValues(typeof(DocumentType))
                               select new
                               {
                                   Value = (int)e,
                                   Text = e.ToString()
                               };
            ViewBag.DocumentType = new SelectList(documenttype, "Value", "Text");

            return View(response);
        }

        [ValidateAntiForgeryToken]
        [ActionName("delete_document")]
        public async Task<ActionResult> Delete_Document([Bind(Include = "Data")] ResponseViewModel<AppointmentDocumentViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var response = await this.AppointmentDocumentService.Delete(model.Data.Id);
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
        public async Task<ActionResult> Feedbacks(long? id, int? page)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

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
                model.Data.IsEmployee = false;
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

        [HttpGet]
        public async Task<ActionResult> Delete_Feedback(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index");

            var response = await this.AppointmentFeedbackService.Get(id.Value);
            return View(response);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_Feedback([Bind(Include = "Data")] ResponseViewModel<AppointmentDocumentViewModel> model)
        {
            var result = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var response = await this.AppointmentFeedbackService.Delete(model.Data.Id);
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

        [NonAction]
        private async Task<List<ServiceLocationViewModel>> GetServiceLocations()
        {
            var response = await this.ServiceLocationService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
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
        private async Task<List<BusinessCustomerViewMdoel>> GetCustomers()
        {
            var response = await this.BusinessCustomerService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessCustomerViewMdoel>();
            }
        }

        [NonAction]
        private async Task<List<BusinessEmployeeViewModel>> GetEmployees()
        {
            var response = await this.BusinessEmployeeService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessEmployeeViewModel>();
            }
        }

        [NonAction]
        private async Task<List<BusinessOfferViewModel>> GetOffers()
        {
            var response = await this.BusinessOfferService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
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
        private async Task<List<BusinessServiceViewModel>> GetBusinessServices()
        {
            var response = await this.BusinessService.Gets(RegisterViewModel.Employee.Id);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessServiceViewModel>();
            }
        }

        [NonAction]
        private async Task<List<BusinessEmployeeViewModel>> GetBusinessEmployee()
        {
            var response = await this.BusinessEmployeeService.Gets(RegisterViewModel.Business.Id,TableType.BusinessId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<BusinessEmployeeViewModel>();
            }
        }

        public async Task<ActionResult> GetCustomerById(long? id)
        {
            if(id.HasValue)
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
    }
}