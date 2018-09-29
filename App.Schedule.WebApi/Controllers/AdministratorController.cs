using System;
using System.Web;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Domains;
using App.Schedule.Context;
using App.Schedule.Domains.Helpers;
using App.Schedule.Domains.ViewModel;
using App.Schedule.WebApi.Services;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Schedule.WebApi.Controllers
{
    [Authorize]
    public class AdministratorController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public AdministratorController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/administrator
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblAdministrators.ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/administrator/5
        [AllowAnonymous]
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid id." });
                else
                {
                    var model = _db.tblAdministrators.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        [AllowAnonymous]
        public IHttpActionResult Get(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                    return Ok(new { status = true, data = "", message = "Please provide a valid email id." });
                else
                {
                    var model = _db.tblAdministrators.Where(d => d.Email == email);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = true, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/administrator/?email=value&password=value
        [AllowAnonymous]
        public async Task<IHttpActionResult> Get(string email, string password, bool hasForgot)
        {
            try
            {
                if (!hasForgot)
                {
                    password = HttpContext.Current.Server.UrlDecode(password);
                    var pass = Security.Encrypt(password, true);
                    var model = _db.tblAdministrators.Where(d => d.Email.ToLower() == email.ToLower() && d.Password
                    == pass).FirstOrDefault();
                    if (model != null)
                    {
                        if (model.IsActive)
                        {
                            model.Password = "";
                            return Ok(new { status = true, data = model, message = "Valid credential" });
                        }
                        else
                        {
                            return Ok(new { status = false, data = model, message = "You can't login. Admin needs to approve your credential." });
                        }
                    }
                    else
                    {
                        return Ok(new { status = false, data = model, message = "Please enter valid credential." });
                    }
                }
                else
                {
                    var model = _db.tblAdministrators.Where(d => d.Email.ToLower() == email.ToLower()).FirstOrDefault();
                    if (model != null)
                    {
                        if (model.IsActive)
                        {
                            var admin = new AdministratorViewModel()
                            {
                                AdministratorId = model.AdministratorId,
                                ContactNumber = model.ContactNumber,
                                Created = model.Created,
                                Email = model.Email,
                                FirstName = model.FirstName,
                                Id = model.Id,
                                IsActive = model.IsActive,
                                IsAdmin = model.IsAdmin,
                                LastName = model.LastName,
                                Password = Security.Decrypt(model.Password,true)
                            };
                            var response = await this.SendMail(admin);
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
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // POST: api/administrator
        public IHttpActionResult Post([FromBody]AdministratorViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                    return Ok(new { status = false, data = "", message = errMessage });
                }

                var isAny = _db.tblAdministrators.Any(d => d.Email.ToLower() == model.Email.ToLower());
                if (isAny)
                    return Ok(new { status = false, data = "", message = "please try another email id." });

                var admin = new tblAdministrator()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = Security.Encrypt(model.Password, true),
                    IsAdmin = model.IsAdmin,
                    IsActive = model.IsActive,
                    ContactNumber = model.ContactNumber,
                    Created = DateTime.Now.ToUniversalTime(),
                    AdministratorId = model.AdministratorId,
                };

                _db.tblAdministrators.Add(admin);
                var response = _db.SaveChanges();

                if (response > 0)
                {
                    return Ok(new { status = true, data = admin, message = "success" });
                }
                return Ok(new { status = false, data = "", message = "failed" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // PUT: api/administrator/5
        public IHttpActionResult Put(long? id, [FromBody]AdministratorViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "please provide a valid id." });
                else
                {
                    var admin = _db.tblAdministrators.Find(id);
                    if (admin != null)
                    {
                        if (admin.Email.ToLower() == model.Email.ToLower())
                        {
                            admin.FirstName = model.FirstName;
                            admin.LastName = model.LastName;
                            //admin.Password = Security.Encrypt(model.Password, true);
                            admin.IsAdmin = model.IsAdmin;
                            //admin.IsActive = model.IsActive;
                            admin.ContactNumber = model.ContactNumber;

                            _db.Entry(admin).State = EntityState.Modified;
                            var response = _db.SaveChanges();
                            if (response > 0)
                                return Ok(new { status = true, data = admin, message = "success" });
                            else
                                return Ok(new { status = false, data = "", message = "failed" });
                        }
                        else
                        {
                            return Ok(new { status = false, data = "", message = "please provide a valid administrator id." });
                        }
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "not a valid data to update. Please provide a valid id." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        public IHttpActionResult Delete(long? id, bool status, DeleteType type)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "please provide a valid admin id." });
                else
                {
                    var admin = _db.tblAdministrators.Find(id);
                    if (admin != null)
                    {
                        admin.IsActive = status;
                        if (type == DeleteType.DeleteRecord)
                        {
                            _db.Entry(admin).State = EntityState.Deleted;
                        }
                        else
                        {
                            _db.Entry(admin).State = EntityState.Modified;
                        }
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = admin, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "failed" });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "not a valid data to update. Please provide a valid id." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "You can not delete. It is in use." });
            }
        }

        [NonAction]
        [AllowAnonymous]
        public async Task<ResponseViewModel<AdministratorViewModel>> RegisterAdmin(AdministratorViewModel model, AppScheduleDbContext dbContext)
        {
            var data = new ResponseViewModel<AdministratorViewModel>();
            try
            {
                var isAny = _db.tblAdministrators.Any(d => d.Email.ToLower() == model.Email.ToLower());
                if (isAny)
                {
                    data.Message = "This email id has already been registered. Try another email id.";
                    data.Status = false;
                }
                else
                {
                    var admin = new tblAdministrator()
                    {
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Password = Security.Encrypt(model.Password, true),
                        IsAdmin = model.IsAdmin,
                        IsActive = model.IsActive,
                        ContactNumber = model.ContactNumber,
                        Created = DateTime.Now.ToUniversalTime(),
                        AdministratorId = model.AdministratorId,
                    };

                    dbContext.tblAdministrators.Add(admin);
                    var response = dbContext.SaveChanges();

                    data.Status = response > 0 ? true : false;
                    data.Message = response > 0 ? "success" : "failed";

                    //Send Mail using Sendgrid
                    await this.SendMail(model);
                }
            }
            catch (Exception ex)
            {
                data.Message = "ex: " + ex.Message.ToString();
                data.Status = false;
            }
            return data;
        }

        [NonAction]
        public ResponseViewModel<AdministratorViewModel> UpdateAdmin(AdministratorViewModel model, AppScheduleDbContext dbContext)
        {
            var data = new ResponseViewModel<AdministratorViewModel>();
            try
            {
                var admin = _db.tblAdministrators.Find(model.Id);
                if (admin != null)
                {
                    if (admin.Email.ToLower() == model.Email.ToLower())
                    {
                        var password = Security.Decrypt(admin.Password, true);
                        if (password == model.OldPassword)
                        {
                            //admin.FirstName = model.FirstName;
                            //admin.LastName = model.LastName;
                            admin.Password = Security.Encrypt(model.Password, true);
                            //admin.IsAdmin = model.IsAdmin;
                            //admin.IsActive = model.IsActive;
                            //admin.ContactNumber = model.ContactNumber;

                            dbContext.Entry(admin).State = EntityState.Modified;
                            var response = dbContext.SaveChanges();
                            data.Status = response > 0 ? true : false;
                            data.Message = response > 0 ? "success" : "failed";
                        }
                        else
                        {
                            data.Message = "Please enter your valid old password.";
                        }
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
        public bool BusinessHourDefaultSetup(long serviceLocationId)
        {
            var result = false;
            try
            {
                var now = DateTime.Now;
                for (var i = 0; i < 7; i++)
                {
                    var businessHour = new tblBusinessHour()
                    {
                        WeekDayId = i,
                        IsStartDay = i == 1 ? true : false,
                        IsHoliday = i == 1 ? true : false,
                        From = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Utc),
                        To = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0, DateTimeKind.Utc),
                        IsSplit1 = false,
                        FromSplit1 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Utc),
                        ToSplit1 = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0, DateTimeKind.Utc),
                        IsSplit2 = false,
                        FromSplit2 = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0, DateTimeKind.Utc),
                        ToSplit2 = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0, DateTimeKind.Utc),
                        ServiceLocationId = serviceLocationId
                    };
                    _db.tblBusinessHours.Add(businessHour);
                }
                var response = _db.SaveChanges();
                if (response > 0)
                    result = true;
                else
                    result = false;
            }
            catch
            {
                result = false;
            }
            return result;
        }

        [NonAction]
        public ResponseViewModel<AdministratorViewModel> DeleteSiteAdmin(long? id, AppScheduleDbContext dbContext)
        {
            var data = new ResponseViewModel<AdministratorViewModel>()
            {
                Status = false
            };
            try
            {
                if (id.HasValue)
                {
                    var siteAdmin = _db.tblAdministrators.Find(id);
                    if (siteAdmin != null)
                    {
                        dbContext.Entry(siteAdmin).State = EntityState.Deleted;
                        var response = dbContext.SaveChanges();
                        data.Data = new AdministratorViewModel() { Email = siteAdmin.Email, Id = siteAdmin.Id };
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
        private async Task<MailResponse> SendMail(AdministratorViewModel model)
        {
            var mailService = new MailService();
            var toMail = new List<string>();
            if (!String.IsNullOrEmpty(model.Email))
            {
                toMail.Add(model.Email);
            }
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
                Subject = "Appointment Scheduler, Site admin login id and password",
                HtmlText = htmlMailBody.ToString(),
                PlainText = ""
            };
            var response = await mailService.SendMail(mailInfomration);
            return response;
        }
    }
}
