﻿using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Context;
using App.Schedule.Domains;
using App.Schedule.Domains.Helpers;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.WebApi.Controllers
{
    /// <summary>
    /// crud API for business.
    /// You can call "api/business" for GET,POST,PUT and DELETE.
    /// </summary>
    [Authorize]
    public class BusinessController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public BusinessController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/business
        public IHttpActionResult Get()
        {
            try
            {
                var model = (from business in _db.tblBusinesses.ToList()
                              join country in _db.tblCountries.ToList()
                              on business.CountryId equals country.Id
                              select new BusinessViewModel
                              {
                                  Add1 = business.Add1,
                                  Add2 = business.Add2,
                                  BusinessCategoryId = business.BusinessCategoryId,
                                  City = business.City,
                                  CountryId =business.CountryId,
                                  CountryName =country.Name,
                                  Created =business.Created,
                                  Email =business.Email,
                                  FaxNumbers =business.Email,
                                  Id =business.Id,
                                  IsActive =business.IsActive,
                                  IsInternational =business.IsInternational,
                                  Logo =business.Logo,
                                  MembershipId =business.MembershipId,
                                  Name =business.Name,
                                  PhoneNumbers =business.PhoneNumbers,
                                  ShortName =business.ShortName,
                                  State =business.State,
                                  TimezoneId =business.TimezoneId,
                                  Website =business.Website,
                                  Zip =business.Zip
                              }).ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // GET: api/business/5
        [AllowAnonymous]
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    var model = (from business in _db.tblBusinesses.ToList()
                                 join country in _db.tblCountries.ToList()
                                 on business.CountryId equals country.Id
                                 join category in _db.tblBusinessCategories.ToList()
                                 on business.BusinessCategoryId equals category.Id
                                 join timezone in _db.tblTimezones.ToList()
                                 on business.TimezoneId equals timezone.Id
                                 where business.Id == id.Value
                                 select new BusinessViewModel
                                 {
                                     Add1 = business.Add1,
                                     Add2 = business.Add2,
                                     BusinessCategoryId = business.BusinessCategoryId,
                                     City = business.City,
                                     CountryId = business.CountryId,
                                     CountryName = country.Name,
                                     Created = business.Created,
                                     Email = business.Email,
                                     FaxNumbers = business.Email,
                                     Id = business.Id,
                                     IsActive = business.IsActive,
                                     IsInternational = business.IsInternational,
                                     Logo = business.Logo,
                                     MembershipId = business.MembershipId,
                                     Name = business.Name,
                                     PhoneNumbers = business.PhoneNumbers,
                                     ShortName = business.ShortName,
                                     State = business.State,
                                     TimezoneId = business.TimezoneId,
                                     Website = business.Website,
                                     Zip = business.Zip,
                                     tblBusinessCategory = new BusinessCategoryViewModel()
                                     {
                                         AdministratorId = category.AdministratorId,
                                         Created = category.Created,
                                         Description = category.Description,
                                         Id = category.Id,
                                         IsActive = category.IsActive,
                                         Name = category.Name,
                                         OrderNumber = category.OrderNumber,
                                         ParentId = category.ParentId,
                                         PictureLink = category.PictureLink,
                                         Type = category.Type
                                     },
                                     tblTimezone = new TimezoneViewModel()
                                     {
                                         AdministratorId = timezone.AdministratorId,
                                         CountryId = timezone.CountryId,
                                         Id = timezone.Id,
                                         IsDST = timezone.IsDST,
                                         Title = timezone.Title,
                                         UtcOffset = timezone.UtcOffset
                                     }
                                 }).SingleOrDefault();
                    if (model != null)
                    {
                        return Ok(new { status = true, data = model, message = "success" });
                    }
                    else
                        return Ok(new { status = false, data = "", message = "Not found" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = ex.Message.ToString() });
            }
        }

        // POST: api/business
        public IHttpActionResult Post([FromBody]BusinessViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var business = new tblBusiness()
                    {
                        Name = model.Name,
                        ShortName = model.ShortName,
                        IsInternational = model.IsInternational,
                        FaxNumbers = model.FaxNumbers,
                        PhoneNumbers = model.PhoneNumbers,
                        Logo = model.Logo,
                        Add1 = model.Add1,
                        Add2 = model.Add2,
                        City = model.City,
                        State = model.State,
                        CountryId = model.CountryId,
                        Email = model.Email,
                        Website = model.Website,
                        Created = DateTime.Now.ToUniversalTime(),
                        IsActive = model.IsActive,
                        Zip = model.Zip,
                        MembershipId = model.MembershipId,
                        BusinessCategoryId = model.BusinessCategoryId,
                        TimezoneId = model.TimezoneId
                    };
                    _db.tblBusinesses.Add(business);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = business });
                    else
                        return Ok(new { status = false, data = "There was a problem." });
                }
                else
                {
                    return Ok(new { status = false, data = "There was a problem." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        // PUT: api/business/5
        public IHttpActionResult Put(long? id, FieldType type, [FromBody]BusinessViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid id." });
                else
                {
                    var business = _db.tblBusinesses.Find(id);
                    if (business != null)
                    {
                        if (type == FieldType.All)
                        {
                            var timeZone = _db.tblTimezones.Where(d => d.Id == model.TimezoneId).FirstOrDefault();
                            var country = _db.tblCountries.Find(timeZone.CountryId);

                            business.Name = model.Name;
                            business.ShortName = model.ShortName;
                            business.IsInternational = model.IsInternational;
                            business.FaxNumbers = model.FaxNumbers;
                            business.PhoneNumbers = model.PhoneNumbers;
                            business.Logo = model.Logo;
                            business.Add1 = model.Add1;
                            business.Add2 = model.Add2;
                            business.City = model.City;
                            business.State = model.State;
                            business.CountryId = country.Id;
                            business.Email = model.Email;
                            business.Website = model.Website;
                            business.Zip = model.Zip;
                            business.MembershipId = model.MembershipId;
                            business.BusinessCategoryId = model.BusinessCategoryId;
                            business.TimezoneId = model.TimezoneId;
                        }
                        else if (type == FieldType.Membership)
                        {
                            business.MembershipId = model.MembershipId;
                        }

                        _db.Entry(business).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = business, message = "Success" });
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

        // DELETE: api/business/5
        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "Please provide a valid ID." });
                else
                {
                    var business = _db.tblBusinesses.Find(id);
                    if (business != null)
                    {
                        business.IsActive = !business.IsActive;
                        _db.Entry(business).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = business });
                        else
                            return Ok(new { status = false, data = "There was a problem to update the data." });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "Not a valid data to update. Please provide a valid id." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "You can not delete. It is in use." });
            }
        }

        [HttpDelete]
        public IHttpActionResult Deactive(int? id, bool status)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    var business = _db.tblBusinesses.Find(id);
                    if (business != null)
                    {
                        business.IsActive = status;
                        _db.Entry(business).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = business, message = "success" });
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

        [NonAction]
        [AllowAnonymous]
        public ResponseViewModel<RegisterViewModel> Register(RegisterViewModel model, AppScheduleDbContext dbContext)
        {
            var data = new ResponseViewModel<RegisterViewModel>();

            var hasName = _db.tblBusinesses.Any(d => d.Name.ToLower() == model.Business.Name.ToLower());
            var hasEmail = _db.tblBusinessEmployees.Any(d => d.Email.ToLower() == model.Business.Email.ToLower());
            if (hasName)
            {
                data.Status = false;
                data.Message = "This business name has been taken. Please try another name.";
            }
            else if (hasEmail)
            {
                data.Status = false;
                data.Message = "This business email has been taken. Please try another email id.";
            }
            else
            {
                try
                {
                    var timeZone = _db.tblTimezones.Where(d => d.Id == model.Business.TimezoneId).FirstOrDefault();
                    var country = _db.tblCountries.Find(timeZone.CountryId);
                    var business = new tblBusiness()
                    {
                        Name = model.Business.Name,
                        ShortName = model.Business.ShortName,
                        IsInternational = model.Business.IsInternational,
                        FaxNumbers = model.Business.FaxNumbers,
                        PhoneNumbers = model.Business.PhoneNumbers,
                        Logo = model.Business.Logo,
                        Add1 = model.Business.Add1,
                        Add2 = model.Business.Add2,
                        City = model.Business.City,
                        State = model.Business.State,
                        CountryId = country.Id,
                        Email = model.Business.Email,
                        Website = model.Business.Website,
                        Created = DateTime.Now.ToUniversalTime(),
                        IsActive = model.Business.IsActive,
                        Zip = model.Business.Zip,
                        MembershipId = model.Business.MembershipId,
                        BusinessCategoryId = model.Business.BusinessCategoryId,
                        TimezoneId = model.Business.TimezoneId
                    };

                    dbContext.tblBusinesses.Add(business);
                    var responseBusiness = dbContext.SaveChanges();

                    var serviceLocation = new tblServiceLocation()
                    {
                        Name = "Main Address",
                        Add1 = model.Business.Add1,
                        Add2 = model.Business.Add2,
                        City = model.Business.City,
                        State = model.Business.State,
                        CountryId = country.Id,
                        Created = DateTime.Now.ToUniversalTime(),
                        IsActive = model.Business.IsActive,
                        Zip = model.Business.Zip,
                        BusinessId = business.Id,
                        TimezoneId = business.TimezoneId,
                        Description = ""
                    };

                    dbContext.tblServiceLocations.Add(serviceLocation);
                    var responseServiceLocation = dbContext.SaveChanges();

                    //Setup default business hours
                    //var businessHourController = new BusinessHourController();
                    //var responseBusinessHour = businessHourController.SetupBusinessHours(serviceLocation.Id);
                    var today = DateTime.Now;
                    var date = new DateTime(today.Year, today.Month, today.Day, 8, 00, 00, DateTimeKind.Utc);
                    for (int i = 0; i < 7; i++)
                    {
                        var businessHour = new tblBusinessHour()
                        {
                            WeekDayId = i,
                            IsStartDay = i == 0 ? true : false,
                            IsHoliday = false,
                            From = date,
                            To = date.AddHours(10),
                            IsSplit1 = false,
                            FromSplit1 = null,
                            ToSplit1 = null,
                            IsSplit2 = false,
                            FromSplit2 = null,
                            ToSplit2 = null,
                            ServiceLocationId = serviceLocation.Id
                        };
                        dbContext.tblBusinessHours.Add(businessHour);
                    }
                    var responseBusinessHour = dbContext.SaveChanges();
                    //End

                    var businessEmployee = new tblBusinessEmployee()
                    {
                        FirstName = model.Employee.FirstName,
                        LastName = model.Employee.LastName,
                        Password = Security.Encrypt(model.Employee.Password, true),
                        Email = model.Employee.Email,
                        STD = model.Employee.STD,
                        PhoneNumber = model.Employee.PhoneNumber,
                        ServiceLocationId = serviceLocation.Id,
                        IsAdmin = true,
                        Created = DateTime.Now.ToUniversalTime(),
                        IsActive = true
                    };
                    dbContext.tblBusinessEmployees.Add(businessEmployee);
                    var responseBusinessEmployee = dbContext.SaveChanges();

                    var businessViewModel = new BusinessViewModel()
                    {
                        Add1 = business.Add1,
                        Add2 = business.Add2,
                        BusinessCategoryId = business.BusinessCategoryId,
                        City = business.City,
                        CountryId = business.CountryId,
                        Created = business.Created,
                        Email = business.Email,
                        FaxNumbers = business.FaxNumbers,
                        Id = business.Id,
                        IsActive = business.IsActive,
                        IsInternational = business.IsInternational,
                        Logo = business.Logo,
                        MembershipId = business.MembershipId,
                        Name = business.Name,
                        PhoneNumbers = business.PhoneNumbers,
                        ShortName = business.ShortName,
                        State = business.State,
                        TimezoneId = business.TimezoneId,
                        Website = business.Website,
                        Zip = business.Zip
                    };

                    var businessEmployeeViewModel = new BusinessEmployeeViewModel()
                    {
                        Created = businessEmployee.Created,
                        Email = businessEmployee.Email,
                        FirstName = businessEmployee.FirstName,
                        Id = businessEmployee.Id,
                        IsActive = businessEmployee.IsActive,
                        IsAdmin = businessEmployee.IsAdmin,
                        LastName = businessEmployee.LastName,
                        PhoneNumber = businessEmployee.PhoneNumber,
                        ServiceLocationId = businessEmployee.ServiceLocationId,
                        STD = businessEmployee.STD
                    };

                    if (responseBusiness > 0 && responseServiceLocation > 0 && responseBusinessEmployee > 0 && responseBusinessHour > 0)
                    {
                        data.Status = true;
                        data.Message = "success";
                        data.Data = new RegisterViewModel();
                        data.Data.Business = new BusinessViewModel();
                        data.Data.Employee = new BusinessEmployeeViewModel();
                        data.Data.Business = businessViewModel;
                        data.Data.Employee = businessEmployeeViewModel;
                        //dbTran.Commit();
                    }
                    else
                    {
                        var reason = "";
                        data.Status = false;
                        if (responseBusiness <= 0)
                            reason += "Business setup issue. ";
                        if (responseServiceLocation <= 0)
                            reason += " Service location issue.";
                        if (responseBusinessEmployee <= 0)
                            reason += " Business employee issue.";
                        if (responseBusinessHour <= 0)
                            reason += " Business hour issue.";

                        data.Message = "Registration failed. reason: " + reason;
                        data.Data.Business = new BusinessViewModel();
                        data.Data.Employee = new BusinessEmployeeViewModel();
                        //dbTran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    data.Status = false;
                    data.Message = "Registration failed. ex: " + ex.Message.ToString();
                    data.Data.Business = new BusinessViewModel();
                    data.Data.Employee = new BusinessEmployeeViewModel();
                    //dbTran.Rollback();
                }
            }
            return data;
        }
    }
}
