using System;
using System.Web;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Context;
using App.Schedule.Domains;
using System.Collections.Generic;
using App.Schedule.Domains.Helpers;
using App.Schedule.Domains.ViewModel;
using System.Threading.Tasks;
using App.Schedule.WebApi.Services;
using System.Text;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    public class BusinessCustomerController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public BusinessCustomerController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/BusinessCustomer
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblBusinessCustomers.ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string email, string password, bool hasForgot)
        {
            try
            {
                if (!hasForgot)
                {
                    var tblBusinessCustomer = new tblBusinessCustomer();
                    password = HttpContext.Current.Server.UrlDecode(password);
                    password = Security.Encrypt(password, true);
                    tblBusinessCustomer = _db.tblBusinessCustomers.Where(d => d.Email.ToLower() == email.ToLower() && d.Password == password && d.IsActive == true).FirstOrDefault();
                    if (tblBusinessCustomer != null)
                    {
                        tblBusinessCustomer.Password = "";
                        var tblServiceLocations = _db.tblServiceLocations.Find(tblBusinessCustomer.ServiceLocationId);
                        if (tblServiceLocations != null)
                        {
                            var tblBusinesses = _db.tblBusinesses.Find(tblServiceLocations.BusinessId);
                            if (tblBusinesses != null)
                            {
                                return Ok(new { status = true, data = new { Customer = tblBusinessCustomer, ServiceLocation = tblServiceLocations, Business = tblBusinesses }, message = "Valid credential" });
                            }
                        }
                        else
                        {
                            return Ok(new { status = false, data = "", message = "Not a valid credential" });
                        }
                    }
                    return Ok(new { status = false, data = "", message = "Not a valid credential" });
                }
                else
                {
                    var model = _db.tblBusinessCustomers.Where(d => d.Email.ToLower() == email.ToLower() && d.IsActive ==true).FirstOrDefault();
                    if (model != null)
                    {
                        if (model.IsActive)
                        {
                            var response = await this.SendMail(model);
                            return Ok(new { status = response.Status, data = model, message = response.Message });
                        }
                        else
                        {
                            return Ok(new { status = false, data = model, message = "You can't get password. Admin needs to approve your credential." });
                        }
                    }
                    return Ok(new { status = false, data = model, message = "Please enter valid credential." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }
        
        // GET: api/businessemployee
        public IHttpActionResult Get(long? id, TableType type)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });

                if (type == TableType.BusinessId)
                {
                    var locations = _db.tblServiceLocations.Where(d => d.BusinessId == id.Value).ToList();
                    var model = new List<BusinessCustomerViewModel>();
                    foreach (var location in locations)
                    {
                        var customers = _db.tblBusinessCustomers.Where(d => d.ServiceLocationId == location.Id).Select(s => new BusinessCustomerViewModel
                        {
                            Created = s.Created,
                            Email = s.Email,
                            FirstName = s.FirstName,
                            Id = s.Id,
                            IsActive = s.IsActive,
                            LastName = s.LastName,
                            Password = s.Password,
                            PhoneNumber = s.PhoneNumber,
                            ServiceLocationId = s.ServiceLocationId,
                            Add1 = s.Add1,
                            Add2 = s.Add2,
                            City = s.City,
                            State = s.City,
                            ProfilePicture = s.ProfilePicture,
                            StdCode = s.StdCode,
                            Zip = s.Zip,
                            ServiceLocation = new ServiceLocationViewModel() { Name = location.Name, Description = location.Description }
                        }).ToList();
                        if (customers.Count > 0)
                        {
                            foreach (var customer in customers)
                            {
                                model.Add(customer);
                            }
                        }
                    }
                    return Ok(new { status = true, data = model, message = "success" });
                }
                else
                {
                    var serviceModel = _db.tblBusinessCustomers.Where(d => d.ServiceLocationId == id.Value).ToList();
                    return Ok(new { status = true, data = serviceModel, message = "success" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }


        // GET: api/BusinessCustomer/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    var model = _db.tblBusinessCustomers.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "Not found." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // POST: api/BusinessCustomer
        public IHttpActionResult Post([FromBody]BusinessCustomerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var businessCustomer = new tblBusinessCustomer()
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        ProfilePicture = model.ProfilePicture,
                        Email = model.Email,
                        StdCode = model.StdCode,
                        PhoneNumber = model.PhoneNumber,
                        Add1 = model.Add1,
                        Add2 = model.Add2,
                        City = model.City,
                        State = model.State,
                        Zip = model.Zip,
                        Password = Security.Encrypt(model.Password, true),
                        IsActive = model.IsActive,
                        Created = model.Created,
                        ServiceLocationId = model.ServiceLocationId
                    };
                    _db.tblBusinessCustomers.Add(businessCustomer);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = businessCustomer, message = "" });
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
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // PUT: api/BusinessCustomer/5,
        public IHttpActionResult Put(long? id, [FromBody]BusinessCustomerViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    if (model != null)
                    {
                        var businessCustomer = _db.tblBusinessCustomers.Find(id);
                        if (businessCustomer != null)
                        {
                            var verifyPass = Security.Encrypt(model.Password, true);
                            if (businessCustomer.Email.ToLower() == model.Email.ToLower() && businessCustomer.Password == verifyPass)
                            {
                                businessCustomer.FirstName = model.FirstName;
                                businessCustomer.LastName = model.LastName;
                                businessCustomer.ProfilePicture = model.ProfilePicture;
                                businessCustomer.Email = model.Email;
                                businessCustomer.StdCode = model.StdCode;
                                businessCustomer.PhoneNumber = model.PhoneNumber;
                                businessCustomer.Add1 = model.Add1;
                                businessCustomer.Add2 = model.Add2;
                                businessCustomer.City = model.City;
                                businessCustomer.State = model.State;
                                businessCustomer.Zip = model.Zip;
                                businessCustomer.ServiceLocationId = model.ServiceLocationId;
                                _db.Entry(businessCustomer).State = EntityState.Modified;
                                var response = _db.SaveChanges();
                                if (response > 0)
                                    return Ok(new { status = true, data = businessCustomer, message = "" });
                                else
                                    return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                            }
                            else
                            {
                                return Ok(new { status = false, data = "", message = "Please provide a valid email id and password to update." });
                            }
                        }
                    }
                    return Ok(new { status = false, data = "Not a valid data to update. Please provide a valid id." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // DELETE: api/BusinessCustomer/5
        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    var businessCustomer = _db.tblBusinessCustomers.Find(id);
                    if (businessCustomer != null)
                    {
                        businessCustomer.IsActive = !businessCustomer.IsActive;
                        _db.Entry(businessCustomer).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessCustomer, message = "" });
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
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // DELETE: api/businessemployee/5
        [HttpDelete]
        public IHttpActionResult Deactive(int? id, bool? status)
        {
            try
            {
                if (!id.HasValue && status.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID and status." });
                else
                {
                    var businessCustomer = _db.tblBusinessCustomers.Find(id);
                    if (businessCustomer != null)
                    {
                        businessCustomer.IsActive = !businessCustomer.IsActive;
                        _db.Entry(businessCustomer).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessCustomer, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "There was a problem to deactive the data." });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid id." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        [NonAction]
        public async Task<ResponseViewModel<BusinessCustomerViewModel>> Register(BusinessCustomerViewModel model)
        {
            var data = new ResponseViewModel<BusinessCustomerViewModel>();
            var hasEmail = _db.tblBusinessCustomers.Any(d => d.Email.ToLower() == model.Email.ToLower() && d.ServiceLocationId == model.ServiceLocationId);
            if (hasEmail)
            {
                data.Status = false;
                data.Message = "This business email has been taken. Please try another email id.";
            }
            else
            {
                var businessCustomer = new tblBusinessCustomer()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = Security.Encrypt(model.Password, true),
                    Email = model.Email,
                    StdCode = model.StdCode,
                    IsActive = model.IsActive,
                    PhoneNumber = model.PhoneNumber,
                    Add1 = model.Add1,
                    Add2 = model.Add2,
                    City = model.City,
                    ProfilePicture = model.ProfilePicture,
                    State = model.State,
                    Zip = model.Zip,
                    Created = DateTime.Now.ToUniversalTime(),
                    ServiceLocationId = model.ServiceLocationId
                };
                _db.tblBusinessCustomers.Add(businessCustomer);
                var response = _db.SaveChanges();
                data.Message = response > 0 ? "success" : "failed";
                data.Status = response > 0 ? true : false;
                data.Data = model;
                await this.SendMail(businessCustomer);
            }
            return data;
        }

        [NonAction]
        public ResponseViewModel<BusinessCustomerViewModel> DeleteCustomer(long? id)
        {
            var data = new ResponseViewModel<BusinessCustomerViewModel>();
            try
            {
                if (id.HasValue)
                {
                    var businessCustomer = _db.tblBusinessCustomers.Find(id);
                    if (businessCustomer != null)
                    {
                        _db.Entry(businessCustomer).State = EntityState.Deleted;
                        var response = _db.SaveChanges();
                        data.Data = new BusinessCustomerViewModel() { Email = businessCustomer.Email, Id = businessCustomer.Id };
                        data.Message = response > 0 ? "success" : "failed";
                        data.Status = response > 0 ? true : false;
                    }
                    else
                    {
                        data.Message = "Not a valid data to update. Please provide a valid id.";
                    }
                }
            }
            catch (Exception ex)
            {
                data.Message = ex.Message.ToString();
            }
            return data;
        }

        [NonAction]
        public ResponseViewModel<BusinessCustomerViewModel> UpdateCustomer(BusinessCustomerViewModel model)
        {
            var data = new ResponseViewModel<BusinessCustomerViewModel>();
            try
            {
                var businessCustomer= _db.tblBusinessCustomers.Find(model.Id);
                if (businessCustomer != null)
                {
                    var password = Security.Decrypt(businessCustomer.Password, true);
                    if (password == model.OldPassword)
                    {
                        if (businessCustomer.Email.ToLower() == model.Email.ToLower())
                        {
                            businessCustomer.Password = Security.Encrypt(model.Password, true);
                            _db.Entry(businessCustomer).State = EntityState.Modified;
                            var response = _db.SaveChanges();
                            data.Message = response > 0 ? "success" : "failed";
                            data.Status = response > 0 ? true : false;
                        }
                        else
                        {
                            data.Message = "Please enter a valid email id.";
                        }
                    }
                    else
                    {
                        data.Message = "Please enter your valid old password.";
                    }
                }
                else
                {
                    data.Message = "It is not a valid information.";
                }
            }
            catch (Exception ex)
            {
                data.Message = "ex: " + ex.Message.ToString();
            }
            return data;
        }

        [NonAction]
        private async Task<MailResponse> SendMail(tblBusinessCustomer model)
        {
            var mailService = new MailService();
            var toMail = new List<string>();
            if (!String.IsNullOrEmpty(model.Email))
            {
                toMail.Add(model.Email);
                var htmlMailBody = new StringBuilder();
                htmlMailBody.Append("<div>");
                htmlMailBody.Append("<div>Hi,</div><br /><br />");
                htmlMailBody.Append("<div>Your Appointment Scheduler Login credential information:</div><br />");
                htmlMailBody.Append(string.Format("<div>Login Id : {0}</div>", model.Email));
                htmlMailBody.Append(string.Format("<div>Password : {0}</div>", Security.Decrypt(model.Password, true)));
                htmlMailBody.Append("<br /><br />");
                htmlMailBody.Append("<h4>Regard's</h4>");
                htmlMailBody.Append("<h3>Appointment Scheduler</h3>");
                htmlMailBody.Append("</div>");

                var mailInfomration = new MailInformation()
                {
                    To = toMail,
                    Subject = "Appointment Scheduler, Business customer login id and password",
                    HtmlText = htmlMailBody.ToString(),
                    PlainText = ""
                };
                var mailResponse = await mailService.SendMail(mailInfomration);
                return mailResponse;
            }
            else
            {
                return new MailResponse() { Status = false, Message = "Please provide valid email id." };
            }
        }
    }
}
