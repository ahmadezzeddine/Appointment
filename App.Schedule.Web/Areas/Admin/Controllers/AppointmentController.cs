using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Web.Models;
using System.Collections.Generic;
using App.Schedule.Domains.Helpers;
using App.Schedule.Web.Helpers;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Web;
using iTextSharp.tool.xml;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class AppointmentController : AppointmentBaseController
    {
        AppointmentViewModel appointmentViewModel;

        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search, AppointmentViewStatus? type)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            model.hasAdd = true;
            ViewBag.type = type.HasValue ? (int)type.Value : 0;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;
            if (result.Status && result.Data != null)
            {
                //var appointmentsModel = result.Data.Where(d => d.BusinessEmployeeId != null && d.BusinessEmployeeId == RegisterViewModel.Employee.Id).ToList();
                var appointmentsModel = result.Data.ToList();
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
                        result.Data = appointmentsModel.Where(d => d.StatusType != (int)StatusType.Canceled && d.StatusType != (int)StatusType.CancelRequest && d.IsActive == true).ToList();
                    }
                }
                else
                {
                    result.Data = appointmentsModel.Where(d => d.StatusType != (int)StatusType.Completed && d.StatusType != (int)StatusType.Canceled && d.IsActive == true).ToList();
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
                    model.Data = data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
        public async Task<ActionResult> DeactiveView(int? page, string search, AppointmentViewStatus? type)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            ViewBag.Total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;
            if (result.Status && result.Data != null)
            {
                var appointmentsModel = result.Data.Where(d => d.BusinessEmployeeId != null && d.BusinessEmployeeId == RegisterViewModel.Employee.Id).ToList();

                result.Data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.Canceled || d.IsActive == false).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = result.Data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = result.Data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
        public async Task<ActionResult> CancelRequest(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            model.hasAdd = true;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;
            if (result.Status && result.Data != null)
            {
                var appointmentsModel = result.Data.Where(d => d.BusinessEmployeeId != null).ToList();
                model.hasAdd = appointmentsModel != null && appointmentsModel.Count <= total ? true : false;

                result.Data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.CancelRequest && d.IsActive == true).ToList().OrderByDescending(d => d.Id).ToList();

                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = result.Data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = result.Data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
            }
            return View(model);
        }

        public async Task<ActionResult> CancelRequestAccepted(int? id)
        {
            var result = new ResponseViewModel<AppointmentFeedbackViewModel>();

            if (!id.HasValue)
            {
                result.Message = "Not a valid appointment.";
            }
            else
            {
                var response = await this.AppointmentService.Cancel(id.Value, StatusType.Canceled, "");
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
            return RedirectToAction("CancelRequest");
        }

        [HttpGet]
        public async Task<ActionResult> Canceled(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            model.hasAdd = true;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;
            if (result.Status && result.Data != null)
            {
                var appointmentsModel = result.Data.Where(d => d.BusinessEmployeeId != null && d.BusinessEmployeeId == RegisterViewModel.Employee.Id).ToList();
                model.hasAdd = appointmentsModel != null && appointmentsModel.Count <= total ? true : false;

                result.Data = appointmentsModel.Where(d => d.StatusType == (int)StatusType.Canceled).ToList().OrderByDescending(d => d.Id).ToList();

                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = result.Data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = result.Data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.EmployeeId = RegisterViewModel.Employee.Id;
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

            var employees = await this.GetBusinessEmployee(RegisterViewModel.Business.Id, TableType.BusinessId);
            //var currentEmployee = employees.Find(d => d.Id == RegisterViewModel.Employee.Id);
            //employees.Remove(currentEmployee);
            ViewBag.BusinessEmployeeId = employees;
            response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
            response.Data.IsActive = true;
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
                if (model.Data.SelectedEmployeeIds == null || model.Data.SelectedEmployeeIds.Count <= 0)
                {
                    result.Status = false;
                    result.Message = "Please select at least one invitee.";
                    return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
                }

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

            var fromHours = await hourHelper.GetHoursOfDay((int)response.Data.StartDate.Value.DayOfWeek);
            ViewBag.FromHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = (response.Data.StartTime.HasValue &&  s.Value.Contains(response.Data.StartTime.Value.ToString("hh:mm tt"))) ? true : false
            });
            ViewBag.ToHours = fromHours.Select(s => new SelectListItem()
            {
                Value = s.Value,
                Text = s.Value,
                Selected = (response.Data.EndTime.HasValue && s.Value == response.Data.EndTime.Value.ToString("hh:mm tt")) ? true : false
            });
            response.Data.StartDate = response.Data.StartDate.Value;
            response.Data.EndDate = response.Data.StartDate.Value;
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
            //var appointmentInvitees = await this.GetBusinessEmployee(id.Value, TableType.AppointmentInvitee);
            //var currentEmployee = employees.Find(d => d.Id == RegisterViewModel.Employee.Id);
            //employees.Remove(currentEmployee);
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
                    if (model.Data.SelectedEmployeeIds == null || model.Data.SelectedEmployeeIds.Count <= 0)
                    {
                        result.Status = false;
                        result.Message = "Please select at least one invitee.";
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
                var data = result.Data.Where(d => d.BusinessEmployeeId == RegisterViewModel.Employee.Id).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.GlobalAppointmentId.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
                    model.Data = data.ToPagedList<AppointmentViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Title.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
            }
            return View(model);
        }

        //[HttpGet]
        //public async Task<ActionResult> InvoicePdf(long? id)
        //{
        //    if (!id.HasValue)
        //        return RedirectToAction("index", "appointment", new { area = "admin" });

        //    var response = await this.AppointmentService.Get(id.Value);
        //    response.Status = true;
        //    response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
        //    response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

        //    var ServiceLocations = await this.GetServiceLocations();
        //    response.Data.ServiceLocationName = ServiceLocations.Find(d => d.Id == response.Data.ServiceLocationId).Name;

        //    var BusinessOffers = await this.GetOffers();
        //    response.Data.BusinessOfferName = BusinessOffers.Find(d => d.Id == response.Data.BusinessOfferId).Name;

        //    var BusinessServices = await this.GetBusinessServices();
        //    response.Data.BusinessServiceName = BusinessServices.Find(d => d.Id == response.Data.BusinessServiceId).Name;

        //    response.Data.StartDate = response.Data.StartDate.Value;
        //    response.Data.EndDate = response.Data.StartDate.Value;

        //    var employees = await this.GetBusinessEmployee(id.Value, TableType.AppointmentInvitee);
        //    ViewBag.BusinessEmployeeId = employees;

        //    var modelData = (List<AppointmentViewModel>)TempData["reportData"];
        //    try
        //    {
        //        if (modelData != null)
        //        {
        //            //Dummy data for Invoice (Bill).
        //            string companyName = "Appointment";
        //            int orderNo = 23565;
        //            var dt = new DataTable();
        //            dt.Columns.AddRange(new DataColumn[5] {
        //                    new DataColumn("ProductId", typeof(string)),
        //                    new DataColumn("Product", typeof(string)),
        //                    new DataColumn("Price", typeof(int)),
        //                    new DataColumn("Quantity", typeof(int)),
        //                    new DataColumn("Total", typeof(int))});
        //            dt.Rows.Add(101, "Sun Glasses", 200, 5, 1000);
        //            dt.Rows.Add(102, "Jeans", 400, 2, 800);
        //            dt.Rows.Add(103, "Trousers", 300, 3, 900);
        //            dt.Rows.Add(104, "Shirts", 550, 2, 1100);

        //            using (StringWriter sw = new StringWriter())
        //            {
        //                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //                {
        //                    StringBuilder sb = new StringBuilder();

        //                    //Generate Invoice (Bill) Header.
        //                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
        //                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Order Sheet</b></td></tr>");
        //                    sb.Append("<tr><td colspan = '2'></td></tr>");
        //                    sb.Append("<tr><td><b>Order No: </b>");
        //                    sb.Append(orderNo);
        //                    sb.Append("</td><td align = 'right'><b>Date: </b>");
        //                    sb.Append(DateTime.Now);
        //                    sb.Append(" </td></tr>");
        //                    sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
        //                    sb.Append(companyName);
        //                    sb.Append("</td></tr>");
        //                    sb.Append("</table>");
        //                    sb.Append("<br />");

        //                    //Generate Invoice (Bill) Items Grid.
        //                    sb.Append("<table border = '1'>");
        //                    sb.Append("<tr>");
        //                    foreach (DataColumn column in dt.Columns)
        //                    {
        //                        sb.Append("<th style = 'background-color: #D20B0C;color:#ffffff'>");
        //                        sb.Append(column.ColumnName);
        //                        sb.Append("</th>");
        //                    }
        //                    sb.Append("</tr>");
        //                    foreach (DataRow row in dt.Rows)
        //                    {
        //                        sb.Append("<tr>");
        //                        foreach (DataColumn column in dt.Columns)
        //                        {
        //                            sb.Append("<td>");
        //                            sb.Append(row[column]);
        //                            sb.Append("</td>");
        //                        }
        //                        sb.Append("</tr>");
        //                    }
        //                    sb.Append("<tr><td align = 'right' colspan = '");
        //                    sb.Append(dt.Columns.Count - 1);
        //                    sb.Append("'>Total</td>");
        //                    sb.Append("<td>");
        //                    sb.Append(dt.Compute("sum(Total)", ""));
        //                    sb.Append("</td>");
        //                    sb.Append("</tr></table>");

        //                    //Export HTML String as PDF.
        //                    var sr = new StringReader(sb.ToString());
        //                    var pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //                    var htmlparser = new HTMLWorker(pdfDoc);
        //                    var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //                    pdfDoc.Open();
        //                    htmlparser.Parse(sr);
        //                    pdfDoc.Close();
        //                    Response.ContentType = "application/pdf";
        //                    var fileName = Convert.ToString(Guid.NewGuid()).Replace('-', new char());
        //                    string header = String.Format("attachment; filename=Invoice_{0}.pdf", fileName);
        //                    Response.AddHeader("content-disposition", header);
        //                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //                    Response.Write(pdfDoc);
        //                    Response.End();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(ex.Message.ToString());
        //    }
        //    TempData["reportData"] = modelData;
        //    return Content("Downloading...");
        //}

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

        [HttpGet]
        public async Task<ActionResult> View(long? id)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var response = await this.AppointmentService.Get(id.Value);
            if (response != null)
            {
                response.Status = true;
                response.Data.BusinessEmployeeId = RegisterViewModel.Employee.Id;
                response.Data.GlobalAppointmentId = String.Format("{0}{1}{2}", DateTime.Now.Ticks, RegisterViewModel.Business.Id, RegisterViewModel.Employee.Id);

                var ServiceLocations = await this.GetServiceLocations();
                var location = ServiceLocations.SingleOrDefault(d => d.Id == response.Data.ServiceLocationId);
                if (location != null)
                {
                    response.Data.ServiceLocationName = location.Name;
                }

                var BusinessOffers = await this.GetOffers();
                var offer = BusinessOffers.SingleOrDefault(d => d.Id == response.Data.BusinessOfferId);
                if (offer != null) {
                    response.Data.BusinessOfferName = offer.Name;
                }

                var BusinessServices = await this.GetBusinessServices();
                var serivce = BusinessServices.SingleOrDefault(d => d.Id == response.Data.BusinessServiceId);
                if (serivce!=null)
                {
                    response.Data.BusinessServiceName = serivce.Name;
                }
                response.Data.StartDate = response.Data.StartDate.Value;
                response.Data.EndDate = response.Data.StartDate.Value;

                var employees = await this.GetBusinessEmployee(id.Value, TableType.AppointmentInvitee);
                ViewBag.BusinessEmployeeId = employees;
                response.Data.StartDate = response.Data.StartDate.Value;
                response.Data.EndDate = response.Data.StartDate.Value;
                return View(response);
            }
            else
            {
                return RedirectToAction("view");
            }
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
        public async Task<ActionResult> Attachments(long? id, int? page)
        {
            if (!id.HasValue)
                return RedirectToAction("index", "appointment", new { area = "admin" });

            var pageNumber = page ?? 1;
            var model = this.ResponseHelper.GetResponse<IPagedList<AppointmentDocumentViewModel>>();
            ViewBag.BackLink = Request.UrlReferrer;
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
            //                  select new
            //                  {
            //                      Value = (int)e,
            //                      Text = e.ToString()
            //                  };
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
            var file = Request.Files.Count;
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

        public List<CheckSelectionBox> CheckListOfAppointmentReportType()
        {
            return new List<CheckSelectionBox>
            {
                 new CheckSelectionBox{Id = (int)StatusType.Confirmed, Name = "Confirmed", Checked = true},
                 new CheckSelectionBox{Id = (int)StatusType.Canceled, Name = "Canceled", Checked = true},
                 new CheckSelectionBox{Id = (int)StatusType.CancelRequest, Name = "Cancel Request", Checked = true},
                 new CheckSelectionBox{Id = (int)StatusType.Completed, Name = "Completed", Checked = true},
                 new CheckSelectionBox{Id = (int)StatusType.Resheduled, Name = "Resheduled", Checked = true},
            };
        }

        [HttpGet]
        public async Task<ActionResult> Reports()
        {
            var model = new ResponseViewModel<ReportViewModel>();
            model.Data = new ReportViewModel();
            model.Data.hasOnlyBusinessEmployeeId = new CheckSelectionBox { Id = 0, Name = "Business All Reports", Checked = true };
            model.Data.ReportTypeId = ReportType.Custom;
            model.Data.AppointmentStatusTypeId = this.CheckListOfAppointmentReportType();
            var modelCustomer = await this.GetCustomers();
            if (modelCustomer != null)
            {
                model.Data.CustomerId = modelCustomer.Select(customer => new CheckSelectionBox
                {
                    Id = customer.Id,
                    Checked = true,
                    Name = string.Format("{0} {1}", customer.FirstName, customer.LastName)
                }).ToList();
            }
            model.Status = true;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Reports([Bind(Include = "Data")] ResponseViewModel<ReportViewModel> model)
        {
            var status = false;
            if (model != null && model.Data != null)
            {
                if (model.Data.AppointmentStatusTypeId == null || model.Data.AppointmentStatusTypeId.Count < 0)
                {
                    model.Message = "Please select any one appointment status.";
                    status = true;
                }
                else if (model.Data.ReportTypeId == ReportType.Custom)
                {
                    if (!model.Data.From.HasValue || !model.Data.To.HasValue)
                    {
                        model.Message = "Please provide valid custom from and to date.";
                        status = true;
                    }
                }
                else
                {
                    model.Status = true;
                    model.Message = "success";
                }
            }
            else
            {
                model.Message = "Please try again later. Not a valid data.";
                status = true;
            }
            if (status)
            {
                model.Data.hasOnlyBusinessEmployeeId = new CheckSelectionBox { Id = 0, Name = "Business All Reports", Checked = true };
                model.Data.ReportTypeId = ReportType.Custom;
                model.Data.AppointmentStatusTypeId = this.CheckListOfAppointmentReportType();
                var modelCustomer = await this.GetCustomers();
                if (modelCustomer != null)
                {
                    model.Data.CustomerId = modelCustomer.Select(customer => new CheckSelectionBox
                    {
                        Id = customer.Id,
                        Checked = true,
                        Name = string.Format("{0} {1}", customer.FirstName, customer.LastName)
                    }).ToList();
                }
                model.Status = false;
                return View(model);
            }
            TempData["appointmentData"] = model.Data;
            return RedirectToAction("ReportsView");
        }
        

        [ActionName("ReportsView")]
        public async Task<ActionResult> ReportsView()
        {
            var reportViewModel = (ReportViewModel)TempData["appointmentData"];

            if (reportViewModel == null)
                return RedirectToAction("Reports");

            var model = this.ResponseHelper.GetResponse<List<AppointmentViewModel>>();
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;

            var result = await AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalAppointment;
            if (result.Status && result.Data != null)
            {
                if (reportViewModel.hasOnlyBusinessEmployeeId.Checked)
                {
                    result.Data = result.Data.Where(d => d.BusinessEmployeeId != null && d.BusinessEmployeeId == RegisterViewModel.Employee.Id).ToList();
                }

                var appointments = new List<AppointmentViewModel>();

                foreach (var customerStatus in reportViewModel.CustomerId)
                {
                    if (customerStatus.Checked)
                    {
                        var checkedDatas = result.Data.Where(d => d.BusinessCustomerId == customerStatus.Id).ToList();
                        appointments.AddRange(checkedDatas);
                    }
                }

                result.Data = appointments;
                appointments = new List<AppointmentViewModel>();
                foreach (var appointStatus in reportViewModel.AppointmentStatusTypeId)
                {
                    if (appointStatus.Checked)
                    {
                        var checkedDatas = result.Data.Where(d => d.StatusType == appointStatus.Id).ToList();
                        appointments.AddRange(checkedDatas);
                    }
                }

                result.Data = appointments;

                var start = DateTime.Now;
                var end = DateTime.Now;

                if (reportViewModel.ReportTypeId == (int)ReportType.Custom)
                {
                    start =  reportViewModel.From.Value;
                    end = reportViewModel.To.Value;
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.ThisWeek)
                {
                    start = start.StartOfWeek(DayOfWeek.Monday);
                    end = end.StartOfWeek(DayOfWeek.Monday).AddDays(7);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.NextWeek)
                {
                    start = start.StartOfWeek(DayOfWeek.Monday).AddDays(7);
                    end = end.StartOfWeek(DayOfWeek.Monday).AddDays(14);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.LastWeek)
                {
                    start = start.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
                    end = end.StartOfWeek(DayOfWeek.Monday);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.ThisMonth)
                {
                    start = start.StartOfMonth();
                    end = end.StartOfMonth().AddMonths(1).AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.NextMonth)
                {
                    start = start.StartOfMonth().AddMonths(1);
                    end = end.StartOfMonth().AddMonths(2).AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.LastMonth)
                {
                    start = start.StartOfMonth().AddMonths(-1);
                    end = end.StartOfMonth().AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.ThisYear)
                {
                    start = start.StartOfYear();
                    end = end.StartOfYear().AddYears(1).AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.NextYear)
                {
                    start = start.StartOfYear().AddYears(1);
                    end = end.StartOfYear().AddYears(2).AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.LastYear)
                {
                    start = start.StartOfYear().AddYears(-1);
                    end = end.StartOfYear().AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.Yesterday)
                {
                    start = DateTime.Now.AddDays(-1);
                    end = DateTime.Now.AddDays(-1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else if (reportViewModel.ReportTypeId == ReportType.Tomorrow)
                {
                    start = DateTime.Now.AddDays(1);
                    end = DateTime.Now.AddDays(1);
                    result.Data = result.Data.Where(d => d.StartDate >= start && d.StartDate <= end).ToList();
                }
                else
                {
                    result.Data = result.Data.Where(d => d.StartDate >= DateTime.Now && d.StartDate <= DateTime.Now).ToList();
                }

                result.Data = result.Data.Where(d => d.IsActive == true).OrderByDescending(d => d.Id).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                model.Data = result.Data;
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
            }
            TempData["reportData"] = model.Data;
            return View(model);
        }

        public ContentResult Excel()
        {
            var model = (List<AppointmentViewModel>)TempData["reportData"];
            try
            {
                var gv = new GridView();
                gv.DataSource = model;
                gv.DataBind();
                Response.ClearContent();
                Response.Buffer = true;
                var fileName = Convert.ToString(Guid.NewGuid()).Replace('-', new char());
                string header = String.Format("attachment; filename={0}.xls", fileName);
                Response.AddHeader("content-disposition", header);
                Response.ContentType = "application/ms-excel";
                Response.Charset = "";
                var objStringWriter = new StringWriter();
                var objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }
            TempData["reportData"] = model;
            return Content("Downloading...");
        }

        public ContentResult Pdf()
        {
            var appointments = (List<AppointmentViewModel>)TempData["reportData"];
            try
            {
                if (appointments != null)
                {
                    using (var sw = new StringWriter())
                    {
                        using (var hw = new HtmlTextWriter(sw))
                        {
                            var sb = new StringBuilder();

                            sb.Append("<table border='1' style='width: 100%; line-height: 0.5em; border: 1px solid #f7f7f7; border-collapse: collapse;'>");
                            sb.Append("<tr>");
                            sb.Append("<td style='background-color: #f7f7f7; padding:10px;'>");
                            sb.Append("<h3>Appointment Scheduler</h3>");
                            sb.Append("<span>summary reports</span>");
                            sb.Append("</td>");
                            sb.Append("<td style='text-align: center;'>");
                            sb.Append("<h3>Summary</h3>");
                            sb.Append("</td>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            //END
                            //START
                            sb.Append("<table border='1' style='width: 100%; line-height: 0.5em; font-size:1em; border: 1px solid #f7f7f7; border-collapse: collapse;'>");
                            sb.Append("<tr style='background-color: #d1d1d1; color:#000; height: 20px;'>");
                            sb.Append("<th colspan='6' style='padding:10px;'>Appointments</th>");
                            sb.Append("</tr>");
                            sb.Append("<tr style='background-color: #f7f7f7;'>");
                            sb.Append("<th style='text-align: center; width:200px; padding:10px;'>");
                            sb.Append("Customer Name");
                            sb.Append("</th>");
                            sb.Append("<th colspan='5' style='padding-left:10px; text-align: left; padding:10px;'>");
                            sb.Append("Summary");
                            sb.Append("</th>");
                            sb.Append("</tr>");
                            sb.Append("<tr style='background-color: #f7f7f7;'>");
                            sb.Append("<th style='vertical-align: middle; width:200px; text-align: center;'>");
                            sb.Append("Date & Time");
                            sb.Append("</th>");
                            sb.Append("<th colspan='5'>");
                            sb.Append("<table border='1' style='width: 100%; line-height: 0.5em; font-size:1em; border: 1px solid #f7f7f7; border-collapse: collapse;'>");
                            sb.Append("<tr>");
                            sb.Append("<th style='text-align: left; padding: 10px;'>Fax Number.</th>");
                            sb.Append("<th style='text-align: left; padding: 10px;'>Fax Number.</th>");
                            sb.Append("<th style='text-align: left; padding: 10px;'>Fax Number.</th>");
                            sb.Append("<th style='text-align: left; width:200px; padding: 10px;'>Email</th>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th colspan='3' style='text-align: left; padding:10px;'>Serivce</th>");
                            sb.Append("<th style='text-align: left; padding:10px;'>Location</th>");
                            sb.Append("</tr>");
                            sb.Append("<tr>");
                            sb.Append("<th colspan='3' style='text-align: left; padding:10px;'>Offer Name</th>");
                            sb.Append("<th style='text-align: left; padding:10px;'>Status</th>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            sb.Append("</th>");
                            sb.Append("</tr>");
                            sb.Append("</table>");
                            //END
                            //START
                            var i = 0;
                            foreach (var appointment in appointments)
                            {
                                sb.Append("<table border='1' style='width: 100%; line-height: 0.5em; font-size:1em; border: 1px solid #f7f7f7; border-collapse: collapse;'>");
                                sb.Append("<tr>");
                                sb.Append("<td style='text-align: center;  width:200px; padding:10px; background-color: #d1d1d1; color:#000;'>");
                                sb.Append(appointment.BusinessCustomerName);
                                sb.Append("</td>");
                                sb.Append("<td colspan='5' style='padding-left:10px; text-align: left; padding:10px;'>");
                                sb.Append(appointment.Title);
                                sb.Append("</td>");
                                sb.Append("</tr>");
                                sb.Append("<tr style='background-color: #f7f7f7;'>");
                                sb.Append("<td style='vertical-align: middle; width:200px; text-align: center;'>");
                                var startDate = appointment.StartDate.Value.ToString("MMM, dd yyyy");
                                var startTime = appointment.StartDate.Value.ToString("HH: MM tt");
                                var endTime = appointment.EndDate.Value.ToString("HH: MM tt");
                                var appointTime = string.Format("{0}<br /><br /><br />{1} to {2}", startDate, startTime, endTime);
                                sb.Append(appointTime);
                                sb.Append("</td>");
                                sb.Append("<td colspan='5'>");
                                sb.Append("<table border='1' style='width: 100%; line-height: 0.5em; font-size:1em; border: 1px solid #f7f7f7; border-collapse: collapse;'>");
                                sb.Append("<tr>");
                                var phoneNumber = appointment.BusinessCustomerName;
                                var faxNumber = appointment.BusinessCustomerName;
                                var Email = appointment.BusinessCustomerName;
                                sb.Append("<td style='text-align: left; padding: 10px;'>" + phoneNumber + "</td>");
                                sb.Append("<td style='text-align: left; padding: 10px;'>" + faxNumber + "</td>");
                                sb.Append("<td style='text-align: left; padding: 10px;'>" + faxNumber + "</td>");
                                sb.Append("<td style='text-align: left; width:200px; padding: 10px;'>" + Email + "</td>");
                                sb.Append("</tr>");
                                sb.Append("<tr>");
                                var serviceName = appointment.BusinessServiceName;
                                var locatioName = appointment.ServiceLocationName;
                                var offerName = appointment.BusinessOfferName;
                                sb.Append("<td colspan='3' style='text-align: left; padding:10px;'>" + serviceName + "</td>");
                                sb.Append("<td style='text-align: left; padding:10px;'>" + locatioName + "</td>");
                                sb.Append("</tr>");
                                sb.Append("<tr>");
                                sb.Append("<td colspan='3' style='text-align: left; padding:10px;'>" + offerName + "</td>");
                                var statusType = Enum.GetName(typeof(StatusType), appointment.StatusType);
                                sb.Append("<td style='text-align: left; padding:10px;'>" + statusType + "</td>");
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                sb.Append("</td>");
                                sb.Append("</tr>");
                                sb.Append("</table>");
                                i += 1;
                                if (i >= 3)
                                {
                                    i = 0;
                                    sb.Append("<div style='height:55px'></div>");
                                }
                            }
                            //Export HTML String as PDF.
                            var sr = new StringReader(sb.ToString());
                            var pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 0f);
                            var writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                            pdfDoc.Open();
                            XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                            pdfDoc.Close();
                            Response.ContentType = "application/pdf";
                            var fileName = Convert.ToString(Guid.NewGuid()).Replace('-', new char());
                            string header = String.Format("attachment; filename=Summary_{0}.pdf", fileName);
                            Response.AddHeader("content-disposition", header);
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.Write(pdfDoc);
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message.ToString());
            }
            TempData["reportData"] = appointments;
            return Content("Downloading...");
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
        private async Task<List<BusinessCustomerViewModel>> GetCustomers()
        {
            var response = await this.BusinessCustomerService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
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
            var response = await this.BusinessOfferService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
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
        private async Task<List<BusinessEmployeeViewModel>> GetBusinessEmployee(long? appointmentId, TableType type)
        {
            var employees = new List<BusinessEmployeeViewModel>(); ;
            var response = await this.BusinessEmployeeService.Gets(appointmentId, type);
            if (response != null)
                employees = response.Data;
            return employees;
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
    }
}