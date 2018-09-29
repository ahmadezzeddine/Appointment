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
    public class DocumentCategoryController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public DocumentCategoryController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/businesscategory
        public IHttpActionResult Get()
        {
            try
            {
                var model = _db.tblDocumentCategories.ToList();
                return Ok(new { status = true, data = model, message = "success" });
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/businesscategory/5
        public IHttpActionResult Get(long? id)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "please provide a valid business category id." });
                else
                {
                    var model = _db.tblDocumentCategories.Find(id);
                    if (model != null)
                        return Ok(new { status = true, data = model, message = "success" });
                    else
                        return Ok(new { status = false, data = model, message = "Not found." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // GET: api/businesscategory/5
        public IHttpActionResult Get(long? id, TableType type)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "please provide a valid business category id." });
                else
                {
                    if (type == TableType.BusinessId)
                    {
                        var model = _db.tblDocumentCategories.Find(id);
                        if (model != null)
                            return Ok(new { status = true, data = model, message = "success" });
                        else
                            return Ok(new { status = false, data = model, message = "Not found." });
                    }
                    else
                    {
                        var model = _db.tblDocumentCategories.Find(id);
                        if (model != null)
                            return Ok(new { status = true, data = model, message = "success" });
                        else
                            return Ok(new { status = false, data = model, message = "Not found." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // POST: api/businesscategory
        public IHttpActionResult Post([FromBody]DocumentCategoryViewModel model)
        {
            try
            {
                if (model != null)
                {
                    var documentCategory = new tblDocumentCategory()
                    {
                        Name = model.Name,
                        Type = model.Type,
                        OrderNo = model.OrderNo,
                        PictureLink = model.PictureLink,
                        IsParent = model.IsParent,
                        ParentId = model.ParentId,
                        IsActive = model.IsActive,
                        Created = DateTime.UtcNow,
                    };
                    _db.tblDocumentCategories.Add(documentCategory);
                    var response = _db.SaveChanges();
                    if (response > 0)
                        return Ok(new { status = true, data = documentCategory, message = "success" });
                    else
                        return Ok(new { status = false, data = "", message = "failed" });
                }
                else
                {
                    return Ok(new { status = false, data = "", message = "please provide a valid information." });
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // PUT: api/businesscategory/5
        public IHttpActionResult Put(long? id, [FromBody]DocumentCategoryViewModel model)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "please provide a valid business category id." });
                else
                {
                    var documentCategory = _db.tblDocumentCategories.Find(id);
                    if (documentCategory != null)
                    {
                        documentCategory.Name = model.Name;
                        documentCategory.Type = model.Type;
                        documentCategory.PictureLink = model.PictureLink;
                        documentCategory.OrderNo = model.OrderNo;
                        documentCategory.IsActive = model.IsActive;
                        documentCategory.IsParent = model.IsParent;
                        documentCategory.ParentId = model.ParentId;

                        _db.Entry(documentCategory).State = EntityState.Modified;
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = documentCategory, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "failed" });
                    }
                    else
                    {
                        return Ok(new { status = false, data = "", message = "Not a valid data to update. Please provide a valid information." });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "ex: " + ex.Message.ToString() });
            }
        }

        // DELETE: api/businesscategory/5
        public IHttpActionResult Delete(int? id, bool status, DeleteType type)
        {
            try
            {
                if (!id.HasValue)
                    return Ok(new { status = false, data = "", message = "please provide a valid business category id." });
                else
                {
                    var documentCategory = _db.tblDocumentCategories.Find(id);
                    if (documentCategory != null)
                    {
                        documentCategory.IsActive = !documentCategory.IsActive;
                        if (type == DeleteType.DeleteRecord)
                        {
                            _db.Entry(documentCategory).State = EntityState.Deleted;
                        }
                        else
                        {
                            _db.Entry(documentCategory).State = EntityState.Modified;
                        }
                        var response = _db.SaveChanges();
                        if (response > 0)
                            return Ok(new { status = true, data = documentCategory, message = "success" });
                        else
                            return Ok(new { status = false, data = "", message = "failed" });
                    }
                    else
                    {
                        return Ok(new { status = false, message = "Not a valid data to update. Please provide a valid id.", data = "" });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { status = false, data = "", message = "You can not delete. It is in use." });
            }
        }

        //private readonly AppScheduleDbContext _db;

        //public DocumentCategoryController()
        //{
        //    _db = new AppScheduleDbContext();
        //}

        //// GET: api/DocumentCategory
        //public IHttpActionResult Get()
        //{
        //    try
        //    {
        //        var model = _db.tblDocumentCategories.ToList();
        //        return Ok(new { status = true, data = model });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //}

        //// GET: api/DocumentCategory/5
        //public IHttpActionResult Get(long? id)
        //{
        //    try
        //    {
        //        if (!id.HasValue)
        //            return Ok(new { status = false, data = "Please provide valid ID." });
        //        else
        //        {
        //            var model = _db.tblDocumentCategories.Find(id);
        //            if (model != null)
        //                return Ok(new { status = true, data = model });
        //            else
        //                return Ok(new { status = false, data = "Not found." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //}

        //// POST: api/DocumentCategory
        //public IHttpActionResult Post([FromBody]DocumentCategoryViewModel model)
        //{
        //    try
        //    {
        //        if (model != null)
        //        {
        //            var documentCategory = new tblDocumentCategory()
        //            {
        //                Created = model.Created.ToUniversalTime(),
        //                IsActive = model.IsActive,
        //                IsParent = model.IsParent,
        //                Name = model.Name,
        //                OrderNo = model.OrderNo,
        //                ParentId = model.ParentId,
        //                PictureLink = model.PictureLink,
        //                Type = model.Type
        //            };
        //            _db.tblDocumentCategories.Add(documentCategory);
        //            var response = _db.SaveChanges();
        //            if (response > 0)
        //                return Ok(new { status = true, data = documentCategory });
        //            else
        //                return Ok(new { status = false, data = "There was a problem." });
        //        }
        //        else
        //        {
        //            return Ok(new { status = false, data = "There was a problem." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //}

        //// PUT: api/DocumentCategory/5
        //public IHttpActionResult Put(long? id, [FromBody]DocumentCategoryViewModel model)
        //{
        //    try
        //    {
        //        if (!id.HasValue)
        //            return Ok(new { status = false, data = "Please provide a valid ID." });
        //        else
        //        {
        //            if (model != null)
        //            {
        //                var documentCategory = _db.tblDocumentCategories.Find(id);
        //                if (documentCategory != null)
        //                {
        //                    documentCategory.Created = model.Created.ToUniversalTime();
        //                    documentCategory.IsActive = model.IsActive;
        //                    documentCategory.IsParent = model.IsParent;
        //                    documentCategory.Name = model.Name;
        //                    documentCategory.OrderNo = model.OrderNo;
        //                    documentCategory.ParentId = model.ParentId;
        //                    documentCategory.PictureLink = model.PictureLink;
        //                    documentCategory.Type = model.Type;

        //                    _db.Entry(documentCategory).State = EntityState.Modified;
        //                    var response = _db.SaveChanges();
        //                    if (response > 0)
        //                        return Ok(new { status = true, data = documentCategory });
        //                    else
        //                        return Ok(new { status = false, data = "There was a problem to update the data." });
        //                }
        //            }
        //            return Ok(new { status = false, data = "Not a valid data to update. Please provide a valid id." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //}

        //// DELETE: api/DocumentCategory/5
        //public IHttpActionResult Delete(int? id)
        //{
        //    try
        //    {
        //        if (!id.HasValue)
        //            return Ok(new { status = false, data = "Please provide a valid ID." });
        //        else
        //        {
        //            var documentCategory = _db.tblDocumentCategories.Find(id);
        //            if (documentCategory != null)
        //            {
        //                _db.tblDocumentCategories.Remove(documentCategory);
        //                var response = _db.SaveChanges();
        //                if (response > 0)
        //                    return Ok(new { status = true, data = documentCategory });
        //                else
        //                    return Ok(new { status = false, data = "There was a problem to update the data." });
        //            }
        //            else
        //            {
        //                return Ok(new { status = false, data = "Not a valid data to update. Please provide a valid id." });
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //}
    }
}
