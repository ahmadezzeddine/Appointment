using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Web.Models;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Customer.Controllers.Base;
using FullCalendar;
using System.Drawing;

namespace App.Schedule.Web.Areas.Customer.Controllers
{
    public class DashboardController : DashboardBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var appointments = await GetAppointments();
            ViewBag.totalAppointmentCount = appointments.Count();
            ViewBag.totalAppointmentPendingCount = appointments.Where(d => d.StatusType.Value != (int)StatusType.Completed && d.StatusType != (int)StatusType.Canceled).Count();
            ViewBag.totalAppointmentCompletedCount = appointments.Where(d => d.StatusType.Value == (int)StatusType.Completed).Count();
            ViewBag.totalAppointmentCanceledCount = appointments.Where(d => d.StatusType.Value == (int)StatusType.Canceled).Count();
            ViewBag.BusinessHours = await this.GetBusinessHours();
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (this.LogoutStatus())
                return RedirectToAction("Login", "Home", new { area = "Customer" });
            else
                return RedirectToAction("Index", "Dashboard", new { area = "Customer" });
        }

        [NonAction]
        private async Task<List<AppointmentViewModel>> GetAppointments()
        {
            var data = new List<AppointmentViewModel>();
            var response = await this.AppointmentService.Gets(RegisterCustomerViewModel.Customer.Id, TableType.CustomerId);
            if (response.Status)
            {
                data = response.Data.Where(d => d.StatusType != (int)StatusType.Completed && d.IsActive == true && d.BusinessCustomerId != null).ToList();
            }
            return data;
        }

        [HttpGet]
        public async Task<JsonResult> GetDiaryEvents()
        {
            var appointmentModel = await GetAppointments();
            var recurredAppointments = this.RecurreAppointments(appointmentModel).ToArray();
            return Json(recurredAppointments, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private SchedulerModel TransformAppointmentJson(AppointmentViewModel x)
        {
            var appoint = new SchedulerModel
            {
                id = x.Id,
                title = x.Title + " (" + x.BusinessCustomerName.ToUpper() + ")",
                start = x.StartTime,
                end = x.EndTime,
                color = x.BackColor.HasValue ? Color.FromArgb(x.BackColor.Value).ToString() : "#3a87ad",
                textColor = x.TextColor.HasValue ? Color.FromArgb(x.TextColor.Value).ToString() : "#ffffff",
                url = Url.Action("view", "appointment", new { role = "admin", id = x.Id }),
                className = "",
                someKey = x.Id,
                allDay = x.IsAllDayEvent
            };
            return appoint;
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
                        getDifference = (appointment.EndTime.Value - appointment.StartTime.Value).Days;
                        startDate = appointment.StartTime.Value;
                        endDate = appointment.EndTime.Value;
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
                        getDifference = (appointment.EndTime.Value - appointment.StartTime.Value).Days;
                        startDate = appointment.StartTime.Value;
                        endDate = appointment.EndTime.Value;
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
                        getDifference = (appointment.EndTime.Value - appointment.StartTime.Value).Days;
                        startDate = appointment.StartTime.Value;
                        endDate = appointment.EndTime.Value;
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
                        getDifference = (appointment.EndTime.Value - appointment.StartTime.Value).Days;
                        startDate = appointment.StartTime.Value;
                        endDate = appointment.EndTime.Value;
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
                var response = await this.BusinessHourService.Gets(RegisterCustomerViewModel.Customer.ServiceLocationId.Value, TableType.ServiceLocationId);
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