using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Domains;
using App.Schedule.Context;
using App.Schedule.Domains.ViewModel;
using System.Collections.Generic;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    public class AppointmentController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public AppointmentController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/Appointment
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblAppointments.ToList();
                return Ok(new { status = true, data = model, message = "success"});
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: "+ex.Message.ToString() });
            }
        }

        // GET: api/Appointment
        public IHttpActionResult Get(long? id, TableType type)
        {
            try
            {
                var model = new List<tblAppointment>();
                if (id.HasValue && type == TableType.BusinessId)
                {
                    var locations = _db.tblServiceLocations.Where(d => d.BusinessId == id).ToList();
                    locations.ForEach((location) => {
                        var data = _db.tblAppointments.Where(d => d.ServiceLocationId == location.Id).ToList();
                        model.AddRange(data);
                    });
                }
                else if (id.HasValue && type == TableType.EmployeeId)
                {
                    model = _db.tblAppointments.Where(d => d.BusinessEmployeeId == id.Value).ToList();
                }
                else if (id.HasValue && type == TableType.ServiceLocationId)
                {
                    model = _db.tblAppointments.Where(d => d.ServiceLocationId == id.Value).ToList();
                }
                else
                {
                    model = _db.tblAppointments.ToList();
                }
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/Appointment/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data ="", message = "Please provide valid ID." });
                else
                {
                    var model = _db.tblAppointments.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "No records found." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // POST: api/Appointment
        public IHttpActionResult Post([FromBody]AppointmentViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var randomId = new Random().Next(1000000, 99999999);
                    var appointment = new tblAppointment()
                    {
                        GlobalAppointmentId = randomId,
                        BusinessServiceId = model.BusinessServiceId,
                        Title = model.Title,
                        PatternType = model.PatternType,
                        StartTime = model.StartTime,
                        EndTime = model.EndTime,
                        IsRecuring = model.IsRecuring,
                        IsAllDayEvent = model.IsAllDayEvent,
                        TextColor = model.TextColor,
                        BackColor = model.BackColor,
                        RecureEvery = model.RecureEvery,
                        EndAfter = model.EndAfter,
                        EndAfterDate = model.EndAfterDate,
                        StatusType = model.StatusType,
                        CancelReason = model.CancelReason,
                        IsActive = model.IsActive,
                        Created = model.Created,
                        BusinessCustomerId = model.BusinessCustomerId,
                        BusinessEmployeeId = model.BusinessEmployeeId,
                        BusinessOfferId = model.BusinessOfferId,
                        ServiceLocationId = model.ServiceLocationId
                    };
                    _db.tblAppointments.Add(appointment);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = appointment, message = "" });
                    else
                        return Ok(new { status = false, data = "", message = "There was a problem." });
                }
                else
                {
                    return Ok(new { status = false, data = "", message = "There was a problem." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // PUT: api/Appointment/5
        public IHttpActionResult Put(long? id, [FromBody]AppointmentViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    if (model != null)
                    {
                        var appointment = _db.tblAppointments.Find(id);
                        if (appointment != null)
                        {
                            appointment.GlobalAppointmentId = model.GlobalAppointmentId;
                            appointment.BusinessServiceId = model.BusinessServiceId;
                            appointment.Title = model.Title;
                            appointment.PatternType = model.PatternType;
                            appointment.StartTime = model.StartTime;
                            appointment.EndTime = model.EndTime;
                            appointment.IsRecuring = model.IsRecuring;
                            appointment.IsAllDayEvent = model.IsAllDayEvent;
                            appointment.TextColor = model.TextColor;
                            appointment.BackColor = model.BackColor;
                            appointment.RecureEvery = model.RecureEvery;
                            appointment.EndAfter = model.EndAfter;
                            appointment.EndAfterDate = model.EndAfterDate;
                            appointment.StatusType = model.StatusType;
                            appointment.CancelReason = model.CancelReason;
                            appointment.IsActive = model.IsActive;
                            appointment.Created = model.Created;
                            appointment.BusinessCustomerId = model.BusinessCustomerId;
                            appointment.BusinessEmployeeId = model.BusinessEmployeeId;
                            appointment.BusinessOfferId = model.BusinessOfferId;
                            appointment.ServiceLocationId = model.ServiceLocationId;

                            _db.Entry(appointment).State = EntityState.Modified;
                            var response = _db.SaveChanges();
                            if (response > 0)
                                return Ok(new { status = true, data = appointment, message = "success" });
                            else
                                return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                        }
                    }
                    return Ok(new { status = false, data ="", message = "Not a valid data to update. Please provide a valid id." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // DELETE: api/Appointment/5
        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data ="", message = "Please provide a valid ID." });
                else
                {
                    var appointment = _db.tblAppointments.Find(id);
                    if (appointment != null)
                    {
                        appointment.IsActive = !appointment.IsActive;
                        _db.Entry(appointment).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = appointment, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid id." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }
    }
}
