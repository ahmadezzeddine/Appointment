using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using System.Threading.Tasks;
using App.Schedule.Web.Models;
using FullCalendar;
using System.Drawing;
using App.Schedule.Web.Helpers;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class DashboardController : DashboardBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var appointments = await GetAppointments();
            ViewBag.totalAppointmentCount = appointments.Count();
            ViewBag.totalAppointmentPendingCount = appointments.Where(d => d.StatusType.Value != (int)StatusType.Completed && d.StatusType.Value != (int)StatusType.CancelRequest && d.StatusType != (int)StatusType.Canceled && d.IsActive == true).Count();
            ViewBag.totalAppointmentCompletedCount = appointments.Where(d => d.StatusType.Value == (int)StatusType.Completed && d.IsActive == true).Count();
            ViewBag.totalAppointmentDeactiveCount = appointments.Where(d => d.StatusType.Value == (int)StatusType.Canceled && d.StatusType.Value != (int)StatusType.CancelRequest || d.IsActive == false).Count();
            //ViewBag.totalAppointmentCanceledCount = appointments.Where(d => d.StatusType.Value == (int)StatusType.Canceled && d.IsActive == true).Count();
            ViewBag.totalAppointmentCanceledRequestCount = appointments.Where(d => d.StatusType.Value == (int)StatusType.CancelRequest && d.IsActive == true).Count();
            ViewBag.BusinessHours = await this.GetBusinessHours();
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (this.LogoutStatus())
                return RedirectToAction("Login", "Home", new { area = "Admin" });
            else
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
        }

        [NonAction]
        private async Task<List<AppointmentViewModel>> GetAppointments()
        {
            var data = new List<AppointmentViewModel>();
            var response = await this.AppointmentService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response.Status)
            {
                data = response.Data.Where(d => d.BusinessEmployeeId != null).ToList();
            }
            return data;
        }

        [HttpGet]
        public async Task<JsonResult> GetDiaryEvents(DateTime start, DateTime end)
        {
            var appointments = await GetAppointments();
            //var appointmentModel = appointments.Where(d => d.BusinessEmployeeId != null && d.BusinessEmployeeId == RegisterViewModel.Employee.Id && d.IsActive == true).ToList();
            var recurredAppointments = this.RecurreAppointments(appointments).ToArray();
            return Json(recurredAppointments, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private SchedulerModel TransformAppointmentJson(AppointmentViewModel x)
        {
            var appoint = new SchedulerModel
            {
                id = x.Id,
                title = string.Format("{0}, ({1}), {2}", x.Title, x.BusinessCustomerName.ToUpper(), Enum.GetName(typeof(StatusType), x.StatusType.Value)),
                start = x.StartTime.Value.UtcToLocal() ,
                end = x.EndTime.Value.UtcToLocal(),
                color = x.BackColor.HasValue ? Color.FromArgb(x.BackColor.Value).ToString() : SetBackColor(x.StatusType.Value, x.IsActive),
                textColor = x.TextColor.HasValue ? Color.FromArgb(x.TextColor.Value).ToString() : SetTextColor(x.StatusType.Value, x.IsActive),
                url = Url.Action("view", "appointment", new { area = "admin", id = x.Id }),
                className = "",
                someKey = x.Id,
                description = string.Format("{0}, ({1}), {2}", x.Title, x.BusinessCustomerName.ToUpper(), Enum.GetName(typeof(StatusType), x.StatusType.Value)),
                allDay = x.IsAllDayEvent
            };
            return appoint;
        }

        [NonAction]
        private string SetBackColor(int type, bool status)
        {
            var color = "blue";
            if (status == false)
            {
                color = "#f5f5f5";
            }
            else
            {
                if (type == (int)StatusType.Completed)
                {
                    color = "#dff0d8";
                }
                else if (type == (int)StatusType.Confirmed)
                {
                    color = "#d9edf7";
                }
                else if (type == (int)StatusType.CancelRequest)
                {
                    color = "#f2dede";
                }
                else if (type == (int)StatusType.Canceled)
                {
                    color = "#f2dede";
                }
                else if (type == (int)StatusType.Resheduled)
                {
                    color = "yellow";
                }
                else
                {
                    color = "blue";
                }
            }
            return color;
        }

        [NonAction]
        private string SetTextColor(int type, bool status)
        {
            {
                var color = "#000";
                if (status == false)
                {
                    color = "#333333";
                }
                else
                {
                    if (type == (int)StatusType.Completed)
                    {
                        color = "#3c763d";
                    }
                    else if (type == (int)StatusType.Confirmed)
                    {
                        color = "#31708f";
                    }
                    else if (type == (int)StatusType.CancelRequest)
                    {
                        color = "#a94442";
                    }
                    else if (type == (int)StatusType.Canceled)
                    {
                        color = "#a94442";
                    }
                    else if (type == (int)StatusType.Resheduled)
                    {
                        color = "#000";
                    }
                    else
                    {
                        color = "#000";
                    }
                }
                return color;
            }

        }

        [NonAction]
        private List<SchedulerModel> RecurreAppointments(List<AppointmentViewModel> appointmentList)
        {
            var appointments = new List<SchedulerModel>();
            var getDifference = 0;
            var startDate = new DateTime();
            var endDate = new DateTime();
            var schedule = new SchedulerModel();

            appointmentList.ForEach(appointment =>
            {
                switch (appointment.PatternType)
                {
                    case (int)PatternType.Once:
                        schedule = this.TransformAppointmentJson(appointment);
                        appointments.Add(schedule);
                        break;
                    case (int)PatternType.Daily:
                        getDifference = (appointment.EndTime.Value.UtcToLocal() - appointment.StartTime.Value.UtcToLocal()).Days;
                        startDate = appointment.StartTime.Value.UtcToLocal();
                        endDate = appointment.EndTime.Value.UtcToLocal();
                        do
                        {
                            appointment.StartDate = startDate;
                            appointment.StartTime = appointment.StartDate;
                            appointment.EndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, endDate.Hour, endDate.Minute, endDate.Second);
                            appointment.EndTime = appointment.EndDate;
                            getDifference = (endDate - startDate).Days;
                            schedule = this.TransformAppointmentJson(appointment);
                            appointments.Add(schedule);
                            startDate = startDate.AddDays(1);
                        } while (getDifference > 0);
                        break;
                    case (int)PatternType.Weekly:
                        getDifference = (appointment.EndTime.Value.UtcToLocal() - appointment.StartTime.Value.UtcToLocal()).Days;
                        startDate = appointment.StartTime.Value.UtcToLocal();
                        endDate = appointment.EndTime.Value.UtcToLocal();
                        do
                        {
                            appointment.StartDate = startDate;
                            appointment.StartTime = appointment.StartDate;
                            appointment.EndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, endDate.Hour, endDate.Minute, endDate.Second);
                            appointment.EndTime = appointment.EndDate;
                            getDifference = (endDate - startDate).Days;
                            schedule = this.TransformAppointmentJson(appointment);
                            appointments.Add(schedule);
                            startDate = startDate.AddDays(7);
                        } while (getDifference > 0);
                        break;
                    case (int)PatternType.Monthly:
                        getDifference = (appointment.EndTime.Value.UtcToLocal() - appointment.StartTime.Value.UtcToLocal()).Days;
                        startDate = appointment.StartTime.Value.UtcToLocal();
                        endDate = appointment.EndTime.Value.UtcToLocal();
                        do
                        {
                            appointment.StartDate = startDate;
                            appointment.StartTime = appointment.StartDate;
                            appointment.EndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, endDate.Hour, endDate.Minute, endDate.Second);
                            appointment.EndTime = appointment.EndDate;
                            getDifference = (endDate - startDate).Days;
                            schedule = this.TransformAppointmentJson(appointment);
                            appointments.Add(schedule);
                            startDate = startDate.AddMonths(1);
                        } while (getDifference > 0);
                        break;
                    case (int)PatternType.Yearly:
                        getDifference = (appointment.EndTime.Value.UtcToLocal() - appointment.StartTime.Value.UtcToLocal()).Days;
                        startDate = appointment.StartTime.Value.UtcToLocal();
                        endDate = appointment.EndTime.Value.UtcToLocal();
                        do
                        {
                            appointment.StartDate = startDate;
                            appointment.StartTime = appointment.StartDate;
                            appointment.EndDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, endDate.Hour, endDate.Minute, endDate.Second);
                            appointment.EndTime = appointment.EndDate;
                            getDifference = (endDate - startDate).Days;
                            schedule = this.TransformAppointmentJson(appointment);
                            appointments.Add(schedule);
                            startDate = startDate.AddYears(1);
                        } while (getDifference > 0);
                        break;
                    default:
                        break;
                }
            });
            return appointments;
        }

        [HttpGet]
        public async Task<IEnumerable<BusinessHour>> GetBusinessHours()
        {
            var businessHours = new List<BusinessHour>();
            try
            {
                var response = await this.BusinessHourService.Gets(RegisterViewModel.Employee.ServiceLocationId.Value, TableType.ServiceLocationId);
                if (response != null && response.Status)
                {
                    foreach (var hour in response.Data)
                    {
                        if (!hour.IsHoliday)
                        {
                            var dow = new List<DayOfWeek>();
                            dow.Add((DayOfWeek)hour.WeekDayId);
                            var businessHour = new BusinessHour()
                            {
                                Dow = dow,
                                Start = new TimeSpan(hour.From.Ticks),
                                End = new TimeSpan(hour.To.Ticks)
                            };
                            businessHours.Add(businessHour);
                            if (hour.IsSplit1 != null && hour.IsSplit1.Value)
                            {
                                businessHour = new BusinessHour()
                                {
                                    Dow = dow,
                                    Start = new TimeSpan(hour.FromSplit1.Value.Ticks),
                                    End = new TimeSpan(hour.ToSplit1.Value.Ticks)
                                };
                                businessHours.Add(businessHour);
                            }
                            if (hour.IsSplit2 != null && hour.IsSplit2.Value)
                            {
                                businessHour = new BusinessHour()
                                {
                                    Dow = dow,
                                    Start = new TimeSpan(hour.FromSplit2.Value.Ticks),
                                    End = new TimeSpan(hour.ToSplit2.Value.Ticks)
                                };
                                businessHours.Add(businessHour);
                            }
                        }
                    }
                }
                return businessHours;
            }
            catch
            {
                return businessHours;
            }
        }
    }
}