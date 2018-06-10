using System;
using System.Web;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Domains;
using App.Schedule.Context;
using System.Collections.Generic;
using App.Schedule.Domains.Helpers;
using App.Schedule.Domains.ViewModel;
using System.Threading.Tasks;
using App.Schedule.WebApi.Services;
using System.Text;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    public class BusinessEmployeeController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public BusinessEmployeeController()
        {
            _db = new AppScheduleDbContext();
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
                    var model = new List<BusinessEmployeeViewModel>();
                    foreach (var location in locations)
                    {
                        var employees = _db.tblBusinessEmployees.Where(d => d.ServiceLocationId == location.Id).Select(s => new BusinessEmployeeViewModel
                        {
                            Created = s.Created,
                            Email = s.Email,
                            FirstName = s.FirstName,
                            Id = s.Id,
                            IsAdmin = s.IsAdmin,
                            IsActive = s.IsActive,
                            LastName = s.LastName,
                            Password = s.Password,
                            PhoneNumber = s.PhoneNumber,
                            ServiceLocationId = s.ServiceLocationId,
                            STD = s.STD,
                            ServiceLocation = new ServiceLocationViewModel() { Name = location.Name, Description = location.Description }
                        }).ToList();
                        if (employees.Count > 0)
                        {
                            foreach (var employ in employees)
                            {
                                model.Add(employ);
                            }
                        }
                    }
                    return Ok(new { status = true, data = model, message = "success" });
                }
                else if (type == TableType.ServiceLocationId)
                {
                    var employees = _db.tblBusinessEmployees.Where(d => d.ServiceLocationId == id.Value).ToList();
                    return Ok(new { status = true, data = employees, message = "success" });
                }
                else if (type == TableType.AppointmentInvitee)
                {
                    var appointmentInvitees = _db.tblAppointmentInvitees.Where(d => d.AppointmentId == id.Value).Select(s => s.BusinessEmployeeId).ToList();
                    var employees = _db.tblBusinessEmployees.Where(d => appointmentInvitees.Contains(d.Id));
                    return Ok(new { status = true, data = employees, message = "success" });
                }
                else
                {
                    var model = _db.tblBusinessEmployees.ToList();
                    return Ok(new { status = true, data = model, message = "success" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // GET: api/businessemployee/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    var model = _db.tblBusinessEmployees.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "Not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // GET: api/businessemployee/?emailid=value&password=value
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string email, string password, bool hasForgot)
        {
            try
            {
                if (!hasForgot)
                {
                    var loginSession = new LoginSessionViewModel();
                    password = HttpContext.Current.Server.UrlDecode(password);
                    var pass = Security.Encrypt(password, true);
                    loginSession.Employee = _db.tblBusinessEmployees.Where(d => d.Email.ToLower() == email.ToLower() && d.Password
                    == pass && d.IsActive == true).FirstOrDefault();
                    if (loginSession.Employee != null)
                    {
                        loginSession.Employee.Password = "";
                        var serviceLocation = _db.tblServiceLocations.Find(loginSession.Employee.ServiceLocationId);
                        if (serviceLocation != null)
                        {
                            loginSession.ServiceLocation = serviceLocation;
                            loginSession.Business = _db.tblBusinesses.Include(i => i.tblServiceLocations).Include(i => i.tblMembership).SingleOrDefault(d => d.Id == serviceLocation.BusinessId);
                            return Ok(new { status = true, data = loginSession, message = "Valid credential" });
                        }
                        else
                        {
                            return Ok(new { status = false, data = "", message = "Not a valid credential" });
                        }
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "Not a valid credential" });
                    }
                }
                else
                {
                    var model = _db.tblBusinessEmployees.Where(d => d.Email.ToLower() == email.ToLower() && d.IsActive == true).FirstOrDefault();
                    if (model != null)
                    {
                        if (model.IsActive)
                        {
                            model.Password = Security.Decrypt(model.Password, true);
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

        // POST: api/businessemployee
        public IHttpActionResult Post([FromBody]BusinessEmployeeViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var check = _db.tblBusinessEmployees.Any(d => d.Email.ToLower() == model.Email.ToLower() && model.ServiceLocationId == model.ServiceLocationId);
                    if (!check)
                    {
                        var businessEmployee = new tblBusinessEmployee()
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Password = Security.Encrypt(model.Password, true),
                            Email = model.Email,
                            STD = model.STD,
                            PhoneNumber = model.PhoneNumber,
                            ServiceLocationId = model.ServiceLocationId,
                            IsAdmin = model.IsAdmin,
                            Created = DateTime.Now.ToUniversalTime(),
                            IsActive = model.IsActive
                        };
                        _db.tblBusinessEmployees.Add(businessEmployee);
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessEmployee, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "There was a problem." });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "Email id has already been taken, please try another email id with same service location." });
                    }
                }
                else
                {
                    return Ok(new { status = false, data = "", message = "Model is not valid." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // PUT: api/businessemployee/5
        public IHttpActionResult Put(long? id, [FromBody]BusinessEmployeeViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid id." });
                else
                {
                    var businessEmployee = _db.tblBusinessEmployees.Find(id);
                    if (businessEmployee != null)
                    {
                        //if (model.OldPassword != null)
                        //{
                            //if (businessEmployee.Email.ToLower() == model.Email.ToLower() && businessEmployee.Password == verifyPass)
                            if (businessEmployee.Email.ToLower() == model.Email.ToLower())
                            {
                                businessEmployee.FirstName = model.FirstName;
                                businessEmployee.LastName = model.LastName;
                                businessEmployee.STD = model.STD;
                                businessEmployee.PhoneNumber = model.PhoneNumber;
                                //businessEmployee.ServiceLocationId = model.ServiceLocationId;
                                _db.Entry(businessEmployee).State = EntityState.Modified;
                                var response = _db.SaveChanges();
                                if (response > 0)
                                    return Ok(new { status = true, data = businessEmployee, message = "success" });
                                else
                                    return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                            }
                            else
                            {
                                return Ok(new { status = false, data = "", message = "Please provide a valid email id and password to update." });
                            }
                        //}
                        //else
                        //{
                        //    businessEmployee.FirstName = model.FirstName;
                        //    businessEmployee.LastName = model.LastName;
                        //    businessEmployee.STD = model.STD;
                        //    businessEmployee.PhoneNumber = model.PhoneNumber;
                        //    businessEmployee.IsAdmin = model.IsAdmin;
                        //    businessEmployee.ServiceLocationId = model.ServiceLocationId;
                        //    _db.Entry(businessEmployee).State = EntityState.Modified;
                        //    var response = _db.SaveChanges();
                        //    if (response > 0)
                        //        return Ok(new { status = true, data = businessEmployee, message = "success" });
                        //    else
                        //        return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                        //}
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
        public IHttpActionResult Delete(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    var businessEmployee = _db.tblBusinessEmployees.Find(id);
                    if (businessEmployee != null)
                    {
                        _db.Entry(businessEmployee).State = EntityState.Deleted;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessEmployee, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "There was a problem to delete the data." });
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
        public IHttpActionResult Deactive(long? id, bool? status)
        {
            try
            {
                if (!id.HasValue && status.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID and status." });
                else
                {
                    var businessEmployee = _db.tblBusinessEmployees.Find(id);
                    if (businessEmployee != null)
                    {
                        businessEmployee.IsActive = !businessEmployee.IsActive;
                        _db.Entry(businessEmployee).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessEmployee, message = "success" });
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
        [AllowAnonymous]
        public async Task<ResponseViewModel<BusinessEmployeeViewModel>> Register(BusinessEmployeeViewModel model)
        {
            var data = new ResponseViewModel<BusinessEmployeeViewModel>();
            var hasEmail = _db.tblBusinessEmployees.Any(d => d.Email.ToLower() == model.Email.ToLower() && d.ServiceLocationId == model.ServiceLocationId);
            if (hasEmail)
            {
                data.Status = false;
                data.Message = "This business email has been taken. Please try another email id.";
            }
            else
            {
                var businessEmployee = new tblBusinessEmployee()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = Security.Encrypt(model.Password, true),
                    Email = model.Email,
                    STD = model.STD,
                    PhoneNumber = model.PhoneNumber,
                    ServiceLocationId = model.ServiceLocationId,
                    IsAdmin = model.IsAdmin,
                    Created = DateTime.Now.ToUniversalTime(),
                    IsActive = model.IsActive
                };
                _db.tblBusinessEmployees.Add(businessEmployee);
                var response = _db.SaveChanges();
                data.Message = response > 0 ? "success" : "failed";
                data.Status = response > 0 ? true : false;
                data.Data = model;
                businessEmployee.Password = model.Password;
                await this.SendMail(businessEmployee);
            }
            return data;
        }

        [NonAction]
        public ResponseViewModel<AdministratorViewModel> UpdateAdmin(AdministratorViewModel model)
        {
            var data = new ResponseViewModel<AdministratorViewModel>();
            try
            {
                var admin = _db.tblAdministrators.Find(model.Id);
                if (admin != null)
                {
                    if (admin.Email.ToLower() == model.Email.ToLower())
                    {
                        admin.FirstName = model.FirstName;
                        admin.LastName = model.LastName;
                        admin.Password = Security.Encrypt(model.Password, true);
                        admin.IsAdmin = model.IsAdmin;
                        admin.IsActive = model.IsActive;
                        admin.ContactNumber = model.ContactNumber;

                        _db.Entry(admin).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        data.Status = response > 0 ? true : false;
                        data.Message = response > 0 ? "success" : "failed";
                        data.Data = model;
                    }
                    else
                    {
                        data.Message = "please enter a valid email id.";
                    }
                }
                else
                {
                    data.Message = "it is not a valid admin information.";
                }
            }
            catch (Exception ex)
            {
                data.Message = "ex: " + ex.Message.ToString();
            }
            return data;
        }

        [NonAction]
        public ResponseViewModel<BusinessEmployeeViewModel> UpdateEmployee(BusinessEmployeeViewModel model)
        {
            var data = new ResponseViewModel<BusinessEmployeeViewModel>();
            try
            {
                var businessEmployee = _db.tblBusinessEmployees.Find(model.Id);
                if (businessEmployee != null)
                {
                    var password = Security.Decrypt(businessEmployee.Password, true);
                    if (password == model.OldPassword)
                    {
                        if (businessEmployee.Email.ToLower() == model.Email.ToLower())
                        {
                            businessEmployee.Password = Security.Encrypt(model.Password, true);
                            _db.Entry(businessEmployee).State = EntityState.Modified;
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
        public ResponseViewModel<BusinessEmployeeViewModel> DeleteEmployee(long? id)
        {
            var data = new ResponseViewModel<BusinessEmployeeViewModel>()
            {
                Status = false
            };
            try
            {
                if (id.HasValue)
                {
                    var businessEmployee = _db.tblBusinessEmployees.Find(id);
                    if (businessEmployee != null)
                    {
                        _db.Entry(businessEmployee).State = EntityState.Deleted;
                        var response = _db.SaveChanges();
                        data.Data = new BusinessEmployeeViewModel() { Email = businessEmployee.Email, Id = businessEmployee.Id };
                        data.Message = response > 0 ? "success" : "failed";
                        data.Status = response > 0 ? true : false;
                    }
                    else
                    {
                        data.Message = "Not a valid data to delete. Please provide a valid id.";
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
        private async Task<MailResponse> SendMail(tblBusinessEmployee model)
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
                htmlMailBody.Append(string.Format("<div>Password : {0}</div>", model.Password));
                htmlMailBody.Append("<br /><br />");
                htmlMailBody.Append("<h4>Regard's</h4>");
                htmlMailBody.Append("<h3>Appointment Scheduler</h3>");
                htmlMailBody.Append("</div>");

                var mailInfomration = new MailInformation()
                {
                    To = toMail,
                    Subject = "Appointment Scheduler, Business employee login id and password",
                    HtmlText = htmlMailBody.ToString(),
                    PlainText = ""
                };
                return await mailService.SendMail(mailInfomration);
            }
            else
            {
                return new MailResponse() { Status = false, Message = "Please provide valid email id." };
            }
        }
    }
}
