﻿using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Domains;
using App.Schedule.Context;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    public class AppointmentFeedbackController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public AppointmentFeedbackController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/AppointmentFeedback
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblAppointmentFeedbacks.ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = "ex: "+ ex.Message.ToString() });
            }
        }

        // GET: api/AppointmentFeedback/5
        public IHttpActionResult Get(long? id, TableType type)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    dynamic model;
                    if(type == TableType.AppointmentFeedback)
                    {
                        model = _db.tblAppointmentFeedbacks.Where(d => d.AppointmentId == id.Value).ToList();
                    }
                    else
                    {
                        model = _db.tblAppointmentFeedbacks.Find(id);
                    }
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "Not found." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/AppointmentFeedback/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    var model = _db.tblAppointmentFeedbacks.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "Not found." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // POST: api/AppointmentFeedback
        public IHttpActionResult Post([FromBody]AppointmentFeedbackViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var appointmentFeedback = new tblAppointmentFeedback()
                    {
                        BusinessCustomerId = model.BusinessCustomerId,
                        BusinessEmployeeId = model.BusinessEmployeeId,
                        Created = DateTime.UtcNow,
                        Feedback = model.Feedback,
                        IsActive = true,
                        IsEmployee = model.IsEmployee,
                        Rating = model.Rating,
                        AppointmentId = model.AppointmentId
                    };
                    _db.tblAppointmentFeedbacks.Add(appointmentFeedback);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = appointmentFeedback, message = "success" });
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
                return Ok(new { status = true, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // PUT: api/AppointmentFeedback/5
        public IHttpActionResult Put(long? id, [FromBody]AppointmentFeedbackViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    if (model != null)
                    {
                        var appointmentFeedback = _db.tblAppointmentFeedbacks.Find(id);
                        if (appointmentFeedback != null)
                        {
                            appointmentFeedback.BusinessCustomerId = model.BusinessCustomerId;
                            appointmentFeedback.BusinessEmployeeId = model.BusinessEmployeeId;
                            appointmentFeedback.Created = model.Created.ToUniversalTime();
                            appointmentFeedback.Feedback = model.Feedback;
                            appointmentFeedback.IsActive = model.IsActive;
                            appointmentFeedback.IsEmployee = model.IsEmployee;
                            appointmentFeedback.Rating = model.Rating;
                            appointmentFeedback.AppointmentId = model.AppointmentId;

                            _db.Entry(appointmentFeedback).State = EntityState.Modified;
                            var response = _db.SaveChanges();
                            if (response > 0)
                                return Ok(new { status = true, data = appointmentFeedback, message = "success"});
                            else
                                return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                        }
                    }
                    return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid id." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // DELETE: api/AppointmentFeedback/5
        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    var appointmentFeedback = _db.tblAppointmentFeedbacks.Find(id);
                    if (appointmentFeedback != null)
                    {
                        _db.tblAppointmentFeedbacks.Remove(appointmentFeedback);
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = appointmentFeedback, message = "success" });
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
                return Ok(new { status = true, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }
    }
}
