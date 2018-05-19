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
                var model = _db.tblAppointments
                                    .Include(i => i.tblServiceLocation)
                                    .Include(i => i.tblBusinessService)
                                    .Include(i => i.tblBusinessOffer)
                                    .Include(i => i.tblBusinessCustomer).ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/Appointment
        public IHttpActionResult Get(long? id, TableType type)
        {
            try
            {
                var model = new List<AppointmentViewModel>();
                if (id.HasValue && type == TableType.BusinessId)
                {
                    var locations = _db.tblServiceLocations.Where(d => d.BusinessId == id).ToList();
                    if (locations.Count > 0)
                    {
                        locations.ForEach((location) =>
                        {
                            var data = this.GetApppointments().Where(d => d.ServiceLocationId == location.Id).ToList();
                            model.AddRange(data);
                        });
                    }
                }
                else if (id.HasValue && type == TableType.EmployeeId)
                {
                    var appointmentInvitees = _db.tblAppointmentInvitees.Where(d => d.BusinessEmployeeId == id.Value).ToList();
                    var appointments = this.GetApppointments();
                    appointmentInvitees.ForEach((apinvitee) =>
                    {
                        var appointment = appointments.Find(d => d.Id == apinvitee.AppointmentId);
                        model.Add(appointment);
                    });
                }
                else if (id.HasValue && type == TableType.CustomerId)
                {
                    model = this.GetApppointments().Where(d => d.BusinessCustomerId == id.Value).ToList();
                }
                else if (id.HasValue && type == TableType.ServiceLocationId)
                {
                    model = this.GetApppointments().Where(d => d.ServiceLocationId == id.Value).ToList();
                }
                else if (id.HasValue && type == TableType.Payment)
                {
                    var modelPayment = new AppointmentPayViewModel();

                    var modelAppointment = GetApppointments().Find(d => d.Id == id.Value);

                    if (modelAppointment != null)
                    {
                        modelPayment.Id = modelAppointment.Id;
                        modelPayment.Title = modelAppointment.Title;
                        modelPayment.GlobalAppointmentId = modelAppointment.GlobalAppointmentId;
                        modelPayment.BusinessServiceId = modelAppointment.BusinessServiceId;
                        modelPayment.PatternType = modelAppointment.PatternType;
                        modelPayment.IsRecuring = modelAppointment.IsRecuring;
                        modelPayment.IsAllDayEvent = modelAppointment.IsAllDayEvent;
                        modelPayment.TextColor = modelAppointment.TextColor;
                        modelPayment.BackColor = modelAppointment.BackColor;
                        modelPayment.RecureEvery = modelAppointment.RecureEvery;
                        modelPayment.EndAfter = modelAppointment.EndAfter;
                        modelPayment.EndAfterDate = modelAppointment.EndAfterDate;
                        modelPayment.StatusType = modelAppointment.StatusType;
                        modelPayment.CancelReason = modelAppointment.CancelReason;
                        modelPayment.IsActive = modelAppointment.IsActive;
                        modelPayment.StartDate = modelAppointment.StartTime.Value.ToUniversalTime();
                        modelPayment.EndDate = modelAppointment.EndTime.Value.ToUniversalTime();
                        modelPayment.Created = modelAppointment.Created.ToUniversalTime();
                        modelPayment.BusinessCustomerId = modelAppointment.BusinessCustomerId;
                        modelPayment.BusinessEmployeeId = modelAppointment.BusinessEmployeeId;
                        modelPayment.BusinessOfferId = modelAppointment.BusinessOfferId;
                        modelPayment.ServiceLocationId = modelAppointment.ServiceLocationId;
                        modelPayment.StartTime = modelAppointment.StartTime.Value.ToUniversalTime();
                        modelPayment.EndTime = modelAppointment.EndTime.Value.ToUniversalTime();
                    }
                    var invitees = _db.tblAppointmentInvitees.Where(d => d.AppointmentId == id.Value).Select(s => s.BusinessEmployeeId.Value).ToList();
                    if (invitees.Count > 0) modelPayment.SelectedEmployeeIds = invitees;
                    var serviceCharges = _db.tblBusinessServices.Where(d => d.Id == modelAppointment.BusinessServiceId).FirstOrDefault();
                    var payment = _db.tblAppointmentPayments.Where(d => d.AppointmentId == id.Value).FirstOrDefault();
                    if (payment != null)
                    {
                        modelPayment.Payment = new AppointmentPaymentViewModel();
                        modelPayment.Payment.Id = payment.Id;
                        modelPayment.Payment.Amount = payment.Amount;
                        modelPayment.Payment.AppointmentId = id.Value;
                        modelPayment.Payment.BillingType = payment.BillingType;
                        modelPayment.Payment.CardType = payment.CardType;
                        modelPayment.Payment.CCardNumber = payment.CCardNumber;
                        modelPayment.Payment.CCExpirationDate = payment.CCExpirationDate;
                        modelPayment.Payment.CCFirstName = payment.CCFirstName;
                        modelPayment.Payment.CCLastName = payment.CCLastName;
                        modelPayment.Payment.CCSecurityCode = payment.CCSecurityCode;
                        modelPayment.Payment.ChequeNumber = payment.ChequeNumber;
                        modelPayment.Payment.Created = payment.Created;
                        modelPayment.Payment.IsPaid = payment.IsPaid;
                        modelPayment.Payment.PaidDate = payment.PaidDate;
                        modelPayment.Payment.PurchaseOrderNo = payment.PurchaseOrderNo;
                    }
                    else
                    {
                        modelPayment.Payment = new AppointmentPaymentViewModel();
                        if (serviceCharges != null)
                        {
                            modelPayment.Payment.Amount = serviceCharges.Cost != null ? serviceCharges.Cost : payment.Amount;
                            modelPayment.BusinessServiceName = serviceCharges.Name;
                        }
                    }

                    if (model != null)
                        return Ok(new { status = true, data = modelPayment, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "No records found." });
                }
                else
                {
                    model = this.GetApppointments().ToList();
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
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    var model = this.GetApppointments().Find(d => d.Id == id.Value);
                    var invitees = _db.tblAppointmentInvitees.Where(d => d.AppointmentId == id.Value).Select(s => s.BusinessEmployeeId.Value).ToList();
                    if (invitees.Count > 0) model.SelectedEmployeeIds = invitees;
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
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        try
                        {
                            var appointment = new tblAppointment()
                            {
                                GlobalAppointmentId = model.GlobalAppointmentId,
                                BusinessServiceId = model.BusinessServiceId,
                                Title = model.Title,
                                PatternType = model.PatternType,
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
                                StartTime = model.StartTime.Value.ToUniversalTime(),
                                EndTime = model.EndTime.Value.ToUniversalTime(),
                                Created = model.Created.ToUniversalTime(),
                                BusinessCustomerId = model.BusinessCustomerId,
                                BusinessEmployeeId = model.BusinessEmployeeId,
                                BusinessOfferId = model.BusinessOfferId,
                                ServiceLocationId = model.ServiceLocationId
                            };
                            _db.tblAppointments.Add(appointment);
                            var response = _db.SaveChanges();
                            if (response > 0)
                            {
                                model.SelectedEmployeeIds.ForEach((id) =>
                                {
                                    var invitees = new tblAppointmentInvitee()
                                    {
                                        AppointmentId = appointment.Id,
                                        BusinessEmployeeId = id
                                    };
                                    _db.tblAppointmentInvitees.Add(invitees);
                                });
                                _db.SaveChanges();

                                var serviceCost = _db.tblBusinessServices.Find(model.BusinessServiceId).Cost;
                                var payment = new tblAppointmentPayment()
                                {
                                    AppointmentId = appointment.Id,
                                    Amount = serviceCost,
                                    Created = DateTime.UtcNow,
                                    PaidDate = DateTime.UtcNow
                                };
                                _db.tblAppointmentPayments.Add(payment);
                                _db.SaveChanges();
                                trans.Commit();
                                return Ok(new { status = true, data = appointment, message = "" });
                            }
                        }
                        catch
                        {
                            trans.Rollback();
                        }
                    }
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
                        using (var trans = _db.Database.BeginTransaction())
                        {
                            try
                            {
                                var appointment = _db.tblAppointments.Find(id);
                                if (appointment != null)
                                {
                                    appointment.BusinessServiceId = model.BusinessServiceId;
                                    appointment.Title = model.Title;
                                    appointment.PatternType = model.PatternType;
                                    appointment.StartTime = model.StartTime.Value.ToUniversalTime();
                                    appointment.EndTime = model.EndTime.Value.ToUniversalTime();
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
                                    appointment.Created = model.Created.ToUniversalTime();
                                    appointment.BusinessCustomerId = model.BusinessCustomerId;
                                    appointment.BusinessEmployeeId = model.BusinessEmployeeId;
                                    appointment.BusinessOfferId = model.BusinessOfferId;
                                    appointment.ServiceLocationId = model.ServiceLocationId;

                                    _db.Entry(appointment).State = EntityState.Modified;
                                    var response = _db.SaveChanges();

                                    var getInvitees = _db.tblAppointmentInvitees.Where(d => d.AppointmentId == model.Id);

                                    _db.tblAppointmentInvitees.RemoveRange(getInvitees);
                                    _db.SaveChanges();
                                    if (response > 0)
                                    {
                                        model.SelectedEmployeeIds.ForEach((inviteeeId) =>
                                        {
                                            var invitees = new tblAppointmentInvitee()
                                            {
                                                AppointmentId = appointment.Id,
                                                BusinessEmployeeId = inviteeeId
                                            };
                                            _db.tblAppointmentInvitees.Add(invitees);
                                        });
                                        _db.SaveChanges();
                                        trans.Commit();
                                        return Ok(new { status = true, data = appointment, message = "" });
                                    }
                                    else
                                    {
                                        return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                                    }
                                }
                            }
                            catch
                            {
                                trans.Rollback();
                            }
                        }
                    }
                    return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid id." });
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
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
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

        public IHttpActionResult Deactive(int? id, bool status)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    var businessOffer = _db.tblAppointments.Find(id);
                    if (businessOffer != null)
                    {
                        businessOffer.IsActive = status;
                        _db.Entry(businessOffer).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessOffer, message = "success" });
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

        // PUT: api/Appointment/5
        [HttpPut]
        public IHttpActionResult Close(StatusType type, string reason, [FromBody]AppointmentPayViewModel model)
        {
            try
            {
                if (model == null)
                    return Ok(new { status = false, data = "", message = "Please provide a valid information." });
                else
                {
                    using (var trans = _db.Database.BeginTransaction())
                    {
                        var appointment = _db.tblAppointments.Find(model.Id);
                        if (appointment != null)
                        {
                            appointment.StatusType = model.StatusType;
                            _db.Entry(appointment).State = EntityState.Modified;
                            _db.SaveChanges();

                            var payment = _db.tblAppointmentPayments.Where(d => d.AppointmentId == model.Id).FirstOrDefault();
                            if (payment == null) payment = new tblAppointmentPayment();

                            payment.Amount = model.Payment.Amount;
                            payment.AppointmentId = appointment.Id;
                            payment.BillingType = model.Payment.BillingType;
                            payment.CardType = model.Payment.CardType;
                            payment.CCardNumber = model.Payment.CCardNumber;
                            payment.CCExpirationDate = model.Payment.CCExpirationDate;
                            payment.CCFirstName = model.Payment.CCFirstName;
                            payment.CCLastName = model.Payment.CCLastName;
                            payment.CCSecurityCode = model.Payment.CCSecurityCode;
                            payment.ChequeNumber = model.Payment.ChequeNumber;
                            payment.Created = DateTime.UtcNow;
                            payment.IsPaid = model.Payment.IsPaid;
                            payment.PaidDate = DateTime.UtcNow;
                            payment.PurchaseOrderNo = model.Payment.PurchaseOrderNo;
                            _db.Entry(payment).State = EntityState.Modified;

                            int response = 0;
                            if (payment != null)
                            {
                                //add
                                _db.tblAppointmentPayments.Add(payment);
                                response = _db.SaveChanges();
                            }
                            else
                            {
                                //update
                                _db.Entry(payment).State = EntityState.Modified;
                                response = _db.SaveChanges();
                            }
                            //model.Payment = pay;
                            if (response > 0)
                            {
                                trans.Commit();
                                return Ok(new { status = true, data = model, message = "success" });
                            }
                            else
                            {
                                trans.Rollback();
                                return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                            }
                        }
                        return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid id." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        [NonAction]
        public List<AppointmentViewModel> GetApppointments()
        {
            return _db.tblAppointments
                                    .Include(i => i.tblServiceLocation)
                                    .Include(i => i.tblBusinessService)
                                    .Include(i => i.tblBusinessOffer)
                                    .Include(i => i.tblBusinessCustomer).Select(s => new AppointmentViewModel
                                    {
                                        BackColor = s.BackColor,
                                        BusinessCustomerId = s.BusinessCustomerId,
                                        BusinessEmployeeId = s.BusinessEmployeeId,
                                        BusinessOfferId = s.BusinessOfferId,
                                        BusinessServiceId = s.BusinessServiceId,
                                        CancelReason = s.CancelReason,
                                        Created = s.Created,
                                        EndAfter = s.EndAfter,
                                        EndAfterDate = s.EndAfterDate,
                                        EndTime = s.EndTime,
                                        GlobalAppointmentId = s.GlobalAppointmentId,
                                        Id = s.Id,
                                        IsActive = s.IsActive,
                                        IsAllDayEvent = s.IsAllDayEvent,
                                        IsRecuring = s.IsRecuring,
                                        PatternType = s.PatternType,
                                        RecureEvery = s.RecureEvery,
                                        ServiceLocationId = s.ServiceLocationId,
                                        StartTime = s.StartTime,
                                        StatusType = s.StatusType,
                                        TextColor = s.TextColor,
                                        Title = s.Title,
                                        EndDate = s.EndTime.Value,
                                        StartDate = s.StartTime.Value,
                                        BusinessCustomerName = s.tblBusinessCustomer.FirstName + " " + s.tblBusinessCustomer.LastName,
                                        BusinessOfferName = s.tblBusinessOffer.Name,
                                        BusinessServiceName = s.tblBusinessService.Name,
                                        ServiceLocationName = s.tblServiceLocation.Name
                                    }).ToList();
        }
    }
}
