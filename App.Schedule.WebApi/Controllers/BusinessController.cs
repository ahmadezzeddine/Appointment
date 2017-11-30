using App.Schedule.Context;
using App.Schedule.Domains;
using App.Schedule.Domains.Helpers;
using App.Schedule.Domains.ViewModel;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace App.Schedule.WebApi.Controllers
{
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
                var model = _db.tblBusinesses.ToList();
                return Ok(new { status = true, data = model });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        // GET: api/business/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "Please provide valid ID." });
                else
                {
                    var model = _db.tblBusinesses.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model });
                    else
                        return Ok(new { status = false, data = "Not found." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
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
        public IHttpActionResult Put(long? id, [FromBody]BusinessViewModel model)
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
                        business.CountryId = model.CountryId;
                        business.Email = model.Email;
                        business.Website = model.Website;
                        business.Created = DateTime.Now.ToUniversalTime();
                        business.IsActive = model.IsActive;
                        business.Zip = model.Zip;
                        business.MembershipId = model.MembershipId;
                        business.BusinessCategoryId = model.BusinessCategoryId;
                        business.TimezoneId = model.TimezoneId;

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
                return BadRequest(ex.Message.ToString());
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
                return BadRequest(ex.Message.ToString());
            }
        }

        [NonAction]
        [AllowAnonymous]
        public RegisterViewModel Register(RegisterViewModel model, out bool status, out string message)
        {
            var data = new RegisterViewModel();

            var hasName = _db.tblBusinesses.Any(d => d.Name.ToLower() == model.Business.Name.ToLower());
            var hasEmail = _db.tblBusinessEmployees.Any(d => d.Email.ToLower() == model.Business.Email.ToLower());
            if (hasName)
            {
                status = false;
                message = "This business name has been taken. Please try another name.";
            }
            else if (hasEmail)
            {
                status = false;
                message = "This business email has been taken. Please try another email id.";
            }
            else {
                using (System.Data.Entity.DbContextTransaction dbTran = _db.Database.BeginTransaction())
                {
                    try
                    {
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
                            CountryId = model.Business.CountryId,
                            Email = model.Business.Email,
                            Website = model.Business.Website,
                            Created = DateTime.Now.ToUniversalTime(),
                            IsActive = model.Business.IsActive,
                            Zip = model.Business.Zip,
                            MembershipId = model.Business.MembershipId,
                            BusinessCategoryId = model.Business.BusinessCategoryId,
                            TimezoneId = model.Business.TimezoneId
                        };

                        _db.tblBusinesses.Add(business);
                        var responseBusiness = _db.SaveChanges();

                        var serviceLocation = new tblServiceLocation()
                        {
                            Name = "Main Address",
                            Add1 = model.Business.Add1,
                            Add2 = model.Business.Add2,
                            City = model.Business.City,
                            State = model.Business.State,
                            CountryId = model.Business.CountryId.Value,
                            Created = DateTime.Now.ToUniversalTime(),
                            IsActive = model.Business.IsActive,
                            Zip = model.Business.Zip,
                            BusinessId = business.Id,
                            TimezoneId = business.TimezoneId,
                            Description = ""
                        };

                        _db.tblServiceLocations.Add(serviceLocation);
                        var responseServiceLocation = _db.SaveChanges();

                        var businessEmployee = new tblBusinessEmployee()
                        {
                            FirstName = model.Employee.FirstName,
                            LastName = model.Employee.LastName,
                            LoginId = model.Employee.Email,
                            Password = Security.Encrypt(model.Employee.Password, true),
                            Email = model.Employee.Email,
                            STD = model.Employee.STD,
                            PhoneNumber = model.Employee.PhoneNumber,
                            ServiceLocationId = serviceLocation.Id,
                            IsAdmin = true,
                            Created = DateTime.Now.ToUniversalTime(),
                            IsActive = true
                        };
                        _db.tblBusinessEmployees.Add(businessEmployee);
                        var responseBusinessEmployee = _db.SaveChanges();

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
                            LoginId = businessEmployee.LoginId,
                            PhoneNumber = businessEmployee.PhoneNumber,
                            ServiceLocationId = businessEmployee.ServiceLocationId,
                            STD = businessEmployee.STD
                        };

                        if (responseBusiness > 0 && responseServiceLocation > 0 && responseBusinessEmployee > 0)
                        {
                            status = true;
                            message = "Transaction successed.";
                            data.Business = businessViewModel;
                            data.Employee = businessEmployeeViewModel;
                            dbTran.Commit();
                        }
                        else
                        {
                            status = false;
                            message = "Transaction failed.";
                            data.Business = new BusinessViewModel();
                            data.Employee = new BusinessEmployeeViewModel();
                            dbTran.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        message = "ex: " + ex.Message.ToString();
                        data.Business = new BusinessViewModel();
                        data.Employee = new BusinessEmployeeViewModel();
                        dbTran.Rollback();
                    }
                }
            }
            return data;
        }

    }
}
