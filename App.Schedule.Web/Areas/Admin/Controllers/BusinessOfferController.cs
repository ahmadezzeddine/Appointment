﻿using System;
using PagedList;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Areas.Admin.Controllers
{
    public class BusinessOfferController : BusinessOfferBaseController
    {
        [HttpGet]
        public async Task<ActionResult> Index(int? page, string search)
        {
            var model = this.ResponseHelper.GetResponse<IPagedList<BusinessOfferViewModel>>();
            var pageNumber = page ?? 1;
            ViewBag.search = search;
            model.hasAdd = true;
            ViewBag.BusinessId = RegisterViewModel.Business.Id;
            ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;
            ViewBag.EmployeeId = RegisterViewModel.Employee.Id;

            var total = RegisterViewModel.Business.tblMembership.IsUnlimited ? long.MaxValue : RegisterViewModel.Business.tblMembership.TotalOffers;
            var result = await BusinessOfferService.Gets(RegisterViewModel.Employee.Id, TableType.EmployeeId);
            if (result.Status)
            {
                var data = result.Data;
                model.hasAdd = result.Data != null && result.Data.Count <= total ? true : false;
                model.Status = result.Status;
                model.Message = result.Message;
                if (search == null)
                {
                    model.Data = data.ToPagedList<BusinessOfferViewModel>(pageNumber, 10);
                }
                else
                {
                    model.Data = data.Where(d => d.Name.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
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
        public ActionResult Add(long? id)
        {
            if (id.HasValue)
            {
                ViewBag.BusinessId = id.Value;
                var response = this.ResponseHelper.GetResponse<BusinessOfferViewModel>();
                response.Data = new BusinessOfferViewModel()
                {
                    BusinessEmployeeId = id.Value,
                    Code = GenerateRandomOfferCode()
                };
                response.Status = true;
                response.Data.ValidFrom = DateTime.Now;
                response.Data.ValidTo = DateTime.Now;
                return View(response);
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add([Bind(Include = "Data")]ResponseViewModel<BusinessOfferViewModel> model)
        {
            var result = new ResponseViewModel<BusinessOfferViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.BusinessOfferService.Add(model.Data);
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
                ViewBag.BusinessOfferId = id.Value;
                var response = await this.BusinessOfferService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        response.Data.ValidFrom = DateTime.Now;
                        response.Data.ValidTo = DateTime.Now;
                        return View(response);
                    }

                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Data")]ResponseViewModel<BusinessOfferViewModel> model)
        {
            var result = new ResponseViewModel<ServiceLocationViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.BusinessOfferService.Update(model.Data);
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
        public async Task<ActionResult> Delete(long? id)
        {
            if (id.HasValue)
            {
                ViewBag.BusinessOfferId = id.Value;
                var response = await this.BusinessOfferService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        response.Data.ValidFrom = DateTime.Now;
                        response.Data.ValidTo = DateTime.Now;
                        return View(response);
                    }
                }
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Data")]ResponseViewModel<BusinessOfferViewModel> model)
        {
            var result = new ResponseViewModel<BusinessOfferViewModel>();

            var response = await this.BusinessOfferService.Delete(model.Data.Id);
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
        public async Task<ActionResult> Location(long? id, int? page, string search)
        {
            if (id.HasValue)
            {
                var model = this.ResponseHelper.GetResponse<IPagedList<BusinessOfferServiceLocationViewModel>>();
                var pageNumber = page ?? 1;
                ViewBag.search = search;

                ViewBag.Id = id.Value;
                ViewBag.BusinessId = RegisterViewModel.Business.Id;
                ViewBag.ServiceLocationId = RegisterViewModel.Employee.ServiceLocationId;
                ViewBag.EmployeeId = RegisterViewModel.Employee.Id;

                var result = await BusinessOfferServiceLocationService.Gets(id.Value);
                if (result.Status)
                {
                    var data = result.Data;
                    model.Status = result.Status;
                    model.Message = result.Message;
                    if (data != null)
                    {
                        if (search == null)
                        {
                            model.Data = data.ToPagedList<BusinessOfferServiceLocationViewModel>(pageNumber, 10);
                        }
                        else
                        {
                            model.Data = data.Where(d => d.ServiceLocationViewModel.Name.ToLower().StartsWith(search.ToLower())).ToList().ToPagedList(pageNumber, 10);
                        }
                    }
                }
                else
                {
                    model.Status = result.Status;
                    model.Message = result.Message;
                    model.Data = null;
                }
                return View(model);
            }
            return RedirectToAction("index");
        }

        [HttpGet]
        public async Task<ActionResult> AddLocation(long? id)
        {
            if (id.HasValue)
            {
                var response = this.ResponseHelper.GetResponse<BusinessOfferServiceLocationViewModel>();
                var businessOfferResponse = await this.BusinessOfferService.Get(id.Value);

                var ServiceLocations = await this.GetServiceLocations();
                ViewBag.ServiceLocationId = ServiceLocations.Select(s => new SelectListItem()
                {
                    Value = Convert.ToString(s.Id),
                    Text = s.Name
                });

                response.Data = new BusinessOfferServiceLocationViewModel()
                {
                    BusinessOfferId = id.Value,
                    BusinessOfferViewModel = businessOfferResponse.Status ? businessOfferResponse.Data : new BusinessOfferViewModel()
                };
                response.Status = true;
                return View(response);
            }
            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddLocation([Bind(Include = "Data")]ResponseViewModel<BusinessOfferServiceLocationViewModel> model)
        {
            var result = new ResponseViewModel<BusinessOfferServiceLocationViewModel>();

            if (!ModelState.IsValid)
            {
                var errMessage = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(x => x.ErrorMessage));
                result.Status = false;
                result.Message = errMessage;
            }
            else
            {
                var response = await this.BusinessOfferServiceLocationService.Add(model.Data);
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
        public async Task<ActionResult> DeleteLocation(long? id)
        {
            if (id.HasValue)
            {
                ViewBag.Id = id.Value;
                var response = await this.BusinessOfferServiceLocationService.Get(id.Value);
                if (response != null)
                {
                    if (response.Status)
                    {
                        return View(response);
                    }
                }
            }
            return RedirectToAction("location");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLocation([Bind(Include = "Data")]ResponseViewModel<BusinessOfferServiceLocationViewModel> model)
        {
            var result = new ResponseViewModel<BusinessOfferServiceLocationViewModel>();

            var response = await this.BusinessOfferServiceLocationService.Delete(model.Data.Id);
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

        public string GenerateRandomOfferCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        [NonAction]
        private async Task<List<ServiceLocationViewModel>> GetServiceLocations()
        {
            var response = await this.ServiceLocationService.Gets(RegisterViewModel.Business.Id, TableType.BusinessId);
            if (response != null)
            {
                return response.Data;
            }
            else
            {
                return new List<ServiceLocationViewModel>();
            }
        }
    }
}