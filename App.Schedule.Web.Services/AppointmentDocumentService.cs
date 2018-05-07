using System;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Services
{
    public class AppointmentDocumentService : AppointmentUserBaseService, IAppointmentUserService<AppointmentDocumentViewModel>
    {
        public AppointmentDocumentService(string token)
        {
            this.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<AppointmentDocumentViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_DOCUMENT_BYID, id);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<AppointmentDocumentViewModel>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentDocumentViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<AppointmentDocumentViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_DOCUMENT);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentDocumentViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }


        public async Task<ResponseViewModel<List<AppointmentDocumentViewModel>>> Gets(long? id)
        {
            var returnResponse = new ResponseViewModel<List<AppointmentDocumentViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_DOCUMENT_BYBUSINESSIDANDTYPE, id.Value, TableType.AppointmentDocument);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentDocumentViewModel>>(response.Result);
                if(returnResponse!=null && returnResponse.Data != null)
                {
                    returnResponse.Data.ForEach((data) =>
                    {
                        data.DocumentLink = AppointmentUserService.baseUrl + data.DocumentLink.Substring(2);
                    });
                }
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentDocumentViewModel>> Add(AppointmentDocumentViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.POST_APPOINTMENT_DOCUMENT);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentDocumentViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public Task<ResponseViewModel<AppointmentDocumentViewModel>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseViewModel<AppointmentDocumentViewModel>> Delete(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_APPOINTMENT_DOCUMENT, id.Value);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<AppointmentDocumentViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }
        
        public async Task<ResponseViewModel<AppointmentDocumentViewModel>> Update(AppointmentDocumentViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.PUT_APPOINTMENT_DOCUMENT, model.Id);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentDocumentViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }
    }
}
