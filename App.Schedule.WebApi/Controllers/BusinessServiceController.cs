using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using App.Schedule.Context;
using App.Schedule.Domains;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.WebApi.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class BusinessServiceController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public BusinessServiceController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/businessservice
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblBusinessServices.ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }


        // GET: api/businessservice
        public IHttpActionResult Get(long? id, TableType type)
        {
            try
            {
                if (!id.HasValue)
                {
                    return Ok(new { status = true, data = "", message = "Please provide a valid id." });
                }
                if (type == TableType.EmployeeId)
                {
                    var model = _db.tblBusinessServices.Where(emp => emp.EmployeeId == id.Value).ToList();
                    return Ok(new { status = true, data = model, message = "success" });
                }
                else if (type == TableType.BusinessId)
                {
                    var model = (from business in _db.tblBusinesses.Where(d => d.Id == id.Value).ToList()
                                 join location in _db.tblServiceLocations
                                 on business.Id equals location.BusinessId
                                 join employee in _db.tblBusinessEmployees
                                 on location.Id equals employee.ServiceLocationId
                                 join service in _db.tblBusinessServices
                                 on employee.Id equals service.EmployeeId
                                 select service).ToList();
                    return Ok(new { status = true, data = model, message = "success" });
                }
                else
                {
                    return Ok(new { status = true, data = "", message = "no records" });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/businessservice/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide valid ID." });
                else
                {
                    var model = _db.tblBusinessServices.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "Not found." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // POST: api/businessservice
        public IHttpActionResult Post([FromBody]BusinessServiceViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var businessService = new tblBusinessService()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Cost = model.Cost,
                        IsActive = model.IsActive,
                        Created = DateTime.UtcNow,
                        EmployeeId = model.EmployeeId
                    };
                    _db.tblBusinessServices.Add(businessService);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = businessService, message = "success" });
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

        // PUT: api/businessservice/5
        public IHttpActionResult Put(long? id, [FromBody]BusinessServiceViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    if (model != null)
                    {
                        var businessService = _db.tblBusinessServices.Find(id);
                        if (businessService != null)
                        {
                            businessService.Name = model.Name;
                            businessService.Description = model.Description;
                            businessService.Cost = model.Cost;
                            businessService.IsActive = model.IsActive;
                            businessService.Created = DateTime.UtcNow;
                            businessService.EmployeeId = model.EmployeeId;

                            _db.Entry(businessService).State = EntityState.Modified;
                            var response = _db.SaveChanges();
                            if (response > 0)
                                return Ok(new { status = true, data = businessService, message = "success" });
                            else
                                return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
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

        // DELETE: api/businessservice/5
        public IHttpActionResult Delete(int? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "Please provide a valid ID." });
                else
                {
                    var businessService = _db.tblBusinessServices.Find(id);
                    if (businessService != null)
                    {
                        businessService.IsActive = !businessService.IsActive;
                        _db.Entry(businessService).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = businessService, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "There was a problem to update the data." });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "Not a valid data to update. Please provide a valid id." });
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
