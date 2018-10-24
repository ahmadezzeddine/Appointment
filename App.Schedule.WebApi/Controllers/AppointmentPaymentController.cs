using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Domains;
using App.Schedule.Context;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    //[AllowAnonymous]
    public class AppointmentPaymentController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public AppointmentPaymentController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/AppointmentPayment
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblAppointmentPayments.ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = ex.Message.ToString() });
            }
        }

        // GET: api/AppointmentPayment/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid id." });
                else
                {
                    var model = (from appointment in _db.tblAppointments
                                 join payment in _db.tblAppointmentPayments
                                 on appointment.Id equals payment.AppointmentId
                                 join customer in _db.tblBusinessCustomers
                                 on appointment.BusinessCustomerId equals customer.Id
                                 join employee in _db.tblBusinessEmployees
                                 on appointment.BusinessEmployeeId equals employee.Id
                                 where appointment.Id == id.Value
                                 select new CustomerAppointmentPaymentViewModel
                                 {
                                     Add1 = customer.Add1,
                                     Add2 = customer.Add2,
                                     Amount = payment.Amount,
                                     AppointmentId = appointment.Id,
                                     BackColor = appointment.BackColor,
                                     BillingType = payment.BillingType,
                                     BusinessCustomerId = appointment.BusinessCustomerId,
                                     BusinessEmployeeId = appointment.BusinessEmployeeId,
                                     BusinessOfferId = appointment.BusinessOfferId,
                                     BusinessServiceId = appointment.BusinessServiceId,
                                     CancelReason = appointment.CancelReason,
                                     CardType = payment.CardType,
                                     CCardNumber = payment.CCardNumber,
                                     CCExpirationDate = payment.CCExpirationDate,
                                     CCFirstName = payment.CCFirstName,
                                     CCLastName = payment.CCLastName,
                                     CCSecurityCode = payment.CCSecurityCode,
                                     ChequeNumber = payment.ChequeNumber,
                                     City = customer.City,
                                     CustomerId = customer.Id,
                                     Email = customer.Email,
                                     EndAfter = appointment.EndAfter,
                                     EndAfterDate = appointment.EndAfterDate,
                                     EndTime = appointment.EndTime,
                                     FirstName = customer.FirstName,
                                     LastName = customer.LastName,
                                     GlobalAppointmentId = appointment.GlobalAppointmentId,
                                     IsActive = appointment.IsActive,
                                     PhoneNumber = customer.PhoneNumber,
                                     IsAllDayEvent = appointment.IsAllDayEvent,
                                     PaymentId = payment.Id,
                                     IsPaid = payment.IsPaid,
                                     IsRecuring = appointment.IsRecuring,
                                     PaidDate = payment.PaidDate,
                                     PatternType = appointment.PatternType,
                                     ProfilePicture = customer.ProfilePicture,
                                     ServiceLocationId = appointment.ServiceLocationId,
                                     PurchaseOrderNo = payment.PurchaseOrderNo,
                                     RecureEvery = appointment.RecureEvery,
                                     StartTime = appointment.StartTime,
                                     State = customer.State,
                                     StatusType = appointment.StatusType,
                                     StdCode = customer.StdCode,
                                     TextColor = appointment.TextColor,
                                     Title = appointment.Title,
                                     Zip = customer.Zip,
                                     BusinessEmployeeName = employee.FirstName +"-"+ employee.LastName,
                                     BusinessOfferName =appointment.tblBusinessOffer.Name,
                                     BusinessServiceLocationName = appointment.tblServiceLocation.Name,
                                     BusinessServiceName = appointment.tblBusinessService.Name
                                 }).FirstOrDefault();
                    return Ok(new { status = true, data = model, message = "Success" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = ex.Message.ToString() });
            }
        }

        // POST: api/AppointmentPayment
        public IHttpActionResult Post([FromBody]AppointmentPaymentViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var appointmentPayment = new tblAppointmentPayment()
                    {
                        Amount = model.Amount,
                        AppointmentId = model.AppointmentId,
                        BillingType = model.BillingType,
                        CardType = model.CardType,
                        CCardNumber = model.CCardNumber,
                        CCExpirationDate = model.CCExpirationDate,
                        CCFirstName = model.CCFirstName,
                        CCLastName = model.CCLastName,
                        CCSecurityCode = model.CCSecurityCode,
                        ChequeNumber = model.ChequeNumber,
                        Created = model.Created.ToUniversalTime(),
                        IsPaid = model.IsPaid,
                        PaidDate = model.PaidDate.ToUniversalTime(),
                        PurchaseOrderNo = model.PurchaseOrderNo
                    };
                    _db.tblAppointmentPayments.Add(appointmentPayment);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = appointmentPayment, message = "success" });
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
                return Ok(new { status = true, data = "", message = ex.Message.ToString() });
            }
        }

        // PUT: api/AppointmentPayment/5
        public IHttpActionResult Put(long? id, [FromBody]AppointmentPaymentViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "Please provide a valid ID." });
                else
                {
                    if (model != null)
                    {
                        var appointmentPayment = _db.tblAppointmentPayments.Find(id);
                        if (appointmentPayment != null)
                        {
                            appointmentPayment.Amount = model.Amount;
                            appointmentPayment.AppointmentId = model.AppointmentId;
                            appointmentPayment.BillingType = model.BillingType;
                            appointmentPayment.CardType = model.CardType;
                            appointmentPayment.CCardNumber = model.CCardNumber;
                            appointmentPayment.CCExpirationDate = model.CCExpirationDate;
                            appointmentPayment.CCFirstName = model.CCFirstName;
                            appointmentPayment.CCLastName = model.CCLastName;
                            appointmentPayment.CCSecurityCode = model.CCSecurityCode;
                            appointmentPayment.ChequeNumber = model.ChequeNumber;
                            appointmentPayment.Created = model.Created.ToUniversalTime();
                            appointmentPayment.IsPaid = model.IsPaid;
                            appointmentPayment.PaidDate = model.PaidDate.ToUniversalTime();
                            appointmentPayment.PurchaseOrderNo = model.PurchaseOrderNo;

                            _db.Entry(appointmentPayment).State = EntityState.Modified;
                            var response = _db.SaveChanges();
                            if (response > 0)
                                return Ok(new { status = true, data = appointmentPayment, message = "success" });
                            else
                                return Ok(new { status = false, data = "There was a problem to update the data." });
                        }
                    }
                    return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid id." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = ex.Message.ToString() });
            }
        }

        // DELETE: api/AppointmentPayment/5
        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "Please provide a valid id." });
                else
                {
                    var appointmentPayment = _db.tblAppointmentPayments.Find(id);
                    if (appointmentPayment != null)
                    {
                        _db.tblAppointmentPayments.Remove(appointmentPayment);
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = "", message = "success" });
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
                return Ok(new { status = false, data = "", message = "You can not delete. It is in use." });
            }
        }
    }
}
