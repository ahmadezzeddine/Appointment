using System;
using System.IO;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class DocumentCategoryController : DocumentCategoryBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search)
        {
            Session["categoryTitle"] = "Document Category";
            var model = this.ResponseHelper.GetResponse<IPagedList<DocumentCategoryViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            var result = await this.DocumentCategoryService.Gets();
            if (result.Status)
            {
                var data = result.Data.Where(d => d.ParentId == null).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<DocumentCategoryViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
                model.Data = null;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            var response = this.ResponseHelper.GetResponse<DocumentCategoryViewModel>();
            response.Status = true;
            response.Data = new DocumentCategoryViewModel();
            response.Data.IsActive = true;
            return View(response);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<DocumentCategoryViewModel>();
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.DocumentCategoryService.Add(model.Data);
                if (response == null)
                {
                    result.Status = false;
                    result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                }
                else
                {
                    result.Status = response.Status;
                    result.Message = response.Message;
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long? id)
        {
            if (id.HasValue)
            {
                var response = await this.DocumentCategoryService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }

                }
            }
            return RedirectToAction("index");
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<DocumentCategoryViewModel>();
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.DocumentCategoryService.Update(model.Data);
                if (response == null)
                {
                    result.Status = false;
                    result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                }
                else
                {
                    result.Status = response.Status;
                    result.Message = response.Message;
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        private string HandleImageUpload(Stream fileStream, string name, int size, string type)
        {
            var imageString = "";
            try
            {
                var imageBytes = new byte[fileStream.Length];
                var result = fileStream.Read(imageBytes, 0, imageBytes.Length);
                imageString = System.Text.Encoding.ASCII.GetString(imageBytes);
            }
            catch
            {
                imageString = "";
            }

            return imageString;
        }

        [HttpGet]
        public async Task<ActionResult> Deactive(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.DocumentCategoryId = id.Value;
                var response = await this.DocumentCategoryService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactive([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<DocumentCategoryViewModel>();
            var response = await this.DocumentCategoryService.Deactive(model.Data.Id, model.Data.IsActive);
            if (response == null)
            {
                result.Status = false;
                result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
            }
            else
            {
                result.Status = response.Status;
                result.Message = response.Message;
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.DocumentCategoryId = id.Value;
                var response = await this.DocumentCategoryService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<BusinessOfferViewModel>();

            var response = await this.DocumentCategoryService.Delete(model.Data.Id);
            if (response == null)
            {
                result.Status = false;
                result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
            }
            else
            {
                result.Status = response.Status;
                result.Message = response.Message;
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SubIndex(int? page, string search, int? id)
        {
            Session["categoryTitle"] = "Document Sub Category";
            var model = this.ResponseHelper.GetResponse<IPagedList<DocumentCategoryViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;

            if (id.HasValue)
                ViewBag.ParentId = id.Value;

            var result = await this.DocumentCategoryService.Gets();
            if (result.Status)
            {
                var data = result.Data.Where(d => d.ParentId == id.Value).ToList();
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<DocumentCategoryViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Name.ToLower().Contains(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                }
            }
            else
            {
                model.Status = false;
                model.Message = result.Message;
                model.Data = null;
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult SubCreate(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            var model = new ResponseViewModel<DocumentCategoryViewModel>();
            model.Data = new DocumentCategoryViewModel()
            {
                ParentId = id.Value
            };
            model.Data.IsActive = true;
            model.Status = true;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubCreate([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<DocumentCategoryViewModel>();
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.DocumentCategoryService.Add(model.Data);
                if (response != null)
                {
                    result.Status = response.Status;
                    result.Message = response.Message;
                    result.Data = response.Data;
                }
                else
                {
                    result.Status = false;
                    result.Message = "There was a problem. Please try again later.";
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> SubEdit(long? id)
        {
            if (id.HasValue)
            {
                var response = await this.DocumentCategoryService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubEdit([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<DocumentCategoryViewModel>();
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.DocumentCategoryService.Update(model.Data);
                if (response == null)
                {
                    result.Status = false;
                    result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
                }
                else
                {
                    result.Status = response.Status;
                    result.Message = response.Message;
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> SubDeactive(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.DocumentCategoryId = id.Value;
                var response = await this.DocumentCategoryService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubDeactive([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<DocumentCategoryViewModel>();
            var response = await this.DocumentCategoryService.Deactive(model.Data.Id, model.Data.IsActive);
            if (response == null)
            {
                result.Status = false;
                result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
            }
            else
            {
                result.Status = response.Status;
                result.Message = response.Message;
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> SubDelete(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.DocumentCategoryId = id.Value;
                var response = await this.DocumentCategoryService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubDelete([Bind(Include = "Data")] ResponseViewModel<DocumentCategoryViewModel> model)
        {
            var result = new ResponseViewModel<BusinessOfferViewModel>();

            var response = await this.DocumentCategoryService.Delete(model.Data.Id);
            if (response == null)
            {
                result.Status = false;
                result.Message = response != null ? response.Message : "There was a problem. Please try again later.";
            }
            else
            {
                result.Status = response.Status;
                result.Message = response.Message;
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

    }
}