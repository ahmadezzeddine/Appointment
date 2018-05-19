using System;
using System.Linq;
using System.Web.Mvc;
using App.Schedule.Web.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Areas.Employee.Controllers.Base;

namespace App.Schedule.Web.Areas.Employee.Controllers
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
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            if (this.LogoutStatus())
                return RedirectToAction("login", "home", new { area = "employee" });
            else
                return RedirectToAction("index", "dashboard", new { area = "employee" });
        }

        [NonAction]
        private async Task<List<AppointmentViewModel>> GetAppointments()
        {
            var data = new List<AppointmentViewModel>();
            var response = await this.AppointmentService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            if (response.Status)
            {
                data = response.Data;
            }
            return data;
        }

        [HttpGet]
        public async Task<JsonResult> GetDiaryEvents(DateTime start, DateTime end)
        {
            var appointments = await GetAppointments();
            var recurredAppointments = this.RecurreAppointments(appointments).ToArray();
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
                color = x.BackColor,
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
    }
}