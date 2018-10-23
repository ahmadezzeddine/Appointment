using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Admin.Controllers
{
    public class AdminController : AdminBaseController
    {
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = new ServiceDataViewModel<IPagedList<AdministratorViewModel>>();
            try
            {
                Session["HomeLink"] = "Administrator";
                var pageNumber = page ?? 1;
                ViewBag.search = search;
                var response = await this.AdminService.Gets();
                if (response.Status)
                {
                    var data = response.Data;
                    if (search == null)
                    {
                        model.Data = data.ToPagedList<AdministratorViewModel>(pageNumber, 10);
                        return View(model);
                    }
                    else
                    {
                        model.Data = data.Where(d => d.FirstName.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                        return View(model);
                    }
                }
                else
                {
                    model.HasError = !response.Status;
                    model.Error = response.Message;
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new ServiceDataViewModel<AdministratorViewModel>();
            model.Data = new AdministratorViewModel();
            model.Data.IsActive = true;
            model.Data.IsAdmin = true;
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Data")] ServiceDataViewModel<AdministratorViewModel> model)
        {
            var result = new ResponseViewModel<string>();
            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                if (model.Data.Password.Length <= 8)
                {
                    result.Status = false;
                    result.Message = "Password must be greater than 8 character.";
                }
                else
                {
                    model.Data.Created = DateTime.Now.ToUniversalTime();
                    model.Data.AdministratorId = admin.Id;
                    var response = await this.AdminService.Add(model.Data);
                    if (response.Status)
                    {
                        result.Status = true;
                        result.Message = response.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = response.Message != null ? response.Message : "There was a problem. Please try again later.";
                    }
                }
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long? id)
        {
            var model = new ServiceDataViewModel<AdministratorUpdateViewModel>();
            try
            {
                model.HasError = true;
                if (!id.HasValue)
                {
                    model.Error = "Please provide a valid id.";
                }
                else
                {
                    var res = await this.AdminService.Get(id.Value);
                    if (res.Status)
                    {
                        model.HasError = false;
                        model.Data = new AdministratorUpdateViewModel()
                        {
                            Id = res.Data.Id,
                            FirstName = res.Data.FirstName,
                            LastName = res.Data.LastName,
                            ContactNumber = res.Data.ContactNumber,
                            Email = res.Data.Email,
                            IsAdmin = res.Data.IsAdmin,
                            AdministratorId = res.Data.AdministratorId
                        };
                    }
                    else
                    {
                        model.Error = res.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")] ServiceDataViewModel<AdministratorUpdateViewModel> model)
        {
            var result = new ResponseViewModel<string>();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                    result.Status = false;
                    result.Message = errMessage;
                }
                else
                {
                    var adminiUpdateViewModel = new AdministratorViewModel()
                    {
                        Id = model.Data.Id,
                        LastName = model.Data.LastName,
                        FirstName = model.Data.FirstName,
                        Email = model.Data.Email,
                        ContactNumber = model.Data.ContactNumber,
                        IsAdmin = model.Data.IsAdmin,
                        AdministratorId = model.Data.Id
                    };
                    model.Data.AdministratorId = admin.Id;
                    var response = await this.AdminService.Update(adminiUpdateViewModel);
                    if (response.Status)
                    {
                        result.Status = true;
                        result.Message = response.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = response.Message;
                    }
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Update(long? id)
        {
            var model = new ServiceDataViewModel<AdministratorViewModel>();
            try
            {
                model.HasError = true;
                if (!id.HasValue)
                {
                    model.Error = "Please provide a valid id.";
                }
                else
                {
                    var res = await this.AdminService.Get(id.Value);
                    if (res.Status)
                    {
                        model.HasError = false;
                        model.Data = res.Data;
                        model.Data.Password = "";
                    }
                    else
                    {
                        model.Error = res.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update([Bind(Include = "Data")] ServiceDataViewModel<AdministratorViewModel> model)
        {
            var result = new ResponseViewModel<string>();
            try
            {
                if (!ModelState.IsValid)
                {
                    var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                    result.Status = false;
                    result.Message = errMessage;
                }
                else
                {
                    if (model.Data.Id == admin.Id)
                    {
                        if(model.Data.OldPassword == null)
                        {
                            result.Status = false;
                            result.Message = "Old password required.";
                        }
                        else if (model.Data.Password.Length <= 8)
                        {
                            result.Status = false;
                            result.Message = "Password must be greater than 8 character.";
                        }
                        else if (model.Data.Password.ToLower() == model.Data.OldPassword.ToLower())
                        {
                            result.Status = false;
                            result.Message = "Old password and new password cannot be same.";
                        }
                        else
                        {
                            model.Data.AdministratorId = admin.Id;
                            var response = await this.AdminService.UpdatePassword(model.Data);
                            if (response.Status)
                            {
                                result.Status = true;
                                result.Message = response.Message;
                            }
                            else
                            {
                                result.Status = false;
                                result.Message = response.Message;
                            }
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "You cann't change password of other user information.";
                    }
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<ActionResult> Delete(long? id)
        {
            var model = new ServiceDataViewModel<AdministratorViewModel>();
            try
            {
                model.HasError = true;
                if (!id.HasValue)
                {
                    model.Error = "Please provide a valid id.";
                }
                else
                {
                    var res = await this.AdminService.Get(id.Value);
                    if (res.Status)
                    {
                        if (res.Data.Email.ToLower() != this.admin.Email.ToLower())
                        {
                            model.HasError = false;
                            model.Data = res.Data;
                            model.Data.Password = "";
                            model.Data.ConfirmPassword = "";
                        }
                        else
                        {
                            model.Error = "Sorry, You cann't remove yourself.";
                            return RedirectToAction("index");
                        }
                    }
                    else
                    {
                        model.Error = res.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")] ServiceDataViewModel<AdministratorViewModel> model)
        {
            var result = new ResponseViewModel<AdministratorViewModel>();
            try
            {
                if (model.Data != null)
                {
                    if (model.Data.Email.ToLower() != this.admin.Email.ToLower())
                    {
                        var response = await this.AdminService.DeleteEmployee(model.Data);
                        if (response.Status)
                        {
                            result.Status = true;
                            result.Message = response.Message;
                        }
                        else
                        {
                            result.Status = false;
                            result.Message = response.Message;
                        }
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = "Sorry, You cann't remove yourself";
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = "Not a valid data.";
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> Deactive(long? id)
        {
            var model = new ServiceDataViewModel<AdministratorViewModel>();
            try
            {
                model.HasError = true;
                if (!id.HasValue)
                {
                    model.Error = "Please provide a valid id.";
                }
                else
                {
                    var res = await this.AdminService.Get(id.Value);
                    if (res.Status)
                    {
                        model.HasError = false;
                        model.Data = res.Data;
                        model.Data.Password = "";
                        model.Data.ConfirmPassword = "";
                    }
                    else
                    {
                        model.Error = res.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                model.HasError = true;
                model.Error = "There was a problem. Please try again later.";
                model.ErrorDescription = ex.Message.ToString();
            }
            return View(model);
        }

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactive([Bind(Include = "Data")] ServiceDataViewModel<AdministratorViewModel> model)
        {
            var result = new ResponseViewModel<string>();
            try
            {
                if (!string.IsNullOrEmpty(model.Data.Email))
                {
                    var response = await this.AdminService.Deactive(model.Data.Id, model.Data.IsActive);
                    if (response.Status)
                    {
                        result.Status = true;
                        result.Message = response.Message;
                    }
                    else
                    {
                        result.Status = false;
                        result.Message = response.Message;
                    }
                }
                else
                {
                    result.Status = false;
                    result.Message = "Please provide a valid email id.";
                }
            }
            catch
            {
                result.Status = false;
                result.Message = "There was a problem. Please try again later.";
            }
            return Json(new { status = result.Status, message = result.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}