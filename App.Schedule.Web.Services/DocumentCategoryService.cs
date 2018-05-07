using App.Schedule.Domains.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Schedule.Web.Services
{
    public class DocumentCategoryService : AppointmentUserBaseService, IAppointmentUserService<DocumentCategoryViewModel>
    {
        public DocumentCategoryService(string token)
        {
            base.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<DocumentCategoryViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<DocumentCategoryViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_DOCUMENTCATEGORY_BYID, id);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<DocumentCategoryViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "There was a problem. Please try again return. reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<DocumentCategoryViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<DocumentCategoryViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_DOCUMENTCATEGORIES);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<DocumentCategoryViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "There was a problem. Please try again return. reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<DocumentCategoryViewModel>> Add(DocumentCategoryViewModel model)
        {
            var returnResponse = new ResponseViewModel<DocumentCategoryViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.POST_DOCUMENTCATEGORY);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                returnResponse = await base.GetHttpResponse<DocumentCategoryViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "There was a problem. Please try again return. reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<DocumentCategoryViewModel>> Update(DocumentCategoryViewModel model)
        {
            var returnResponse = new ResponseViewModel<DocumentCategoryViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.PUT_DOCUMENTCATEGORY_BYID, model.Id);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<DocumentCategoryViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "There was a problem. Please try again return. reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<DocumentCategoryViewModel>> Delete(long? id)
        {
            var returnResponse = new ResponseViewModel<DocumentCategoryViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please provide a valid admin id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_DOCUMENTCATEGORY, id.Value, false, DeleteType.DeleteRecord);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<DocumentCategoryViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "There was a problem. Please try again return. reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<DocumentCategoryViewModel>> Deactive(long? id, bool status)
        {
            var returnResponse = new ResponseViewModel<DocumentCategoryViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please provide a valid admin id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_DOCUMENTCATEGORY, id.Value, status, DeleteType.UpdateStatus);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<DocumentCategoryViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "There was a problem. Please try again return. reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }
    }
}
