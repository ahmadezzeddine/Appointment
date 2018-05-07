using App.Schedule.Domains.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace App.Schedule.Web.Services
{
    public class AppointmentService : AppointmentUserBaseService, IAppointmentUserService<AppointmentViewModel>
    {
        public AppointmentService(string token)
        {
            this.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<AppointmentViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_BYID, id);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<AppointmentViewModel>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentPayViewModel>> GetPayment(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentPayViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_BYBUSINESSIDANDTYPE, id, TableType.Payment);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<AppointmentPayViewModel>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentDocumentViewModel>> GetAttachments(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentDocumentViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_BYBUSINESSIDANDTYPE, id, TableType.AppointmentDocument);
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

        public async Task<ResponseViewModel<List<AppointmentViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<AppointmentViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentViewModel>>> Gets(long? id, TableType type)
        {
            var returnResponse = new ResponseViewModel<List<AppointmentViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_BYBUSINESSIDANDTYPE, id, (int)type);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    returnResponse = await base.GetHttpResponse<List<AppointmentViewModel>>(response);
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

        public async Task<ResponseViewModel<AppointmentViewModel>> Add(AppointmentViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.POST_APPOINTMENT);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentViewModel>> Deactive(long? id, bool status)
        {
            var returnResponse = new ResponseViewModel<AppointmentViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DEACTIVE_APPOINTMENT, id.Value, status);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<AppointmentViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentViewModel>> Delete(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_APPOINTMENT, id.Value);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<AppointmentViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentViewModel>> Update(AppointmentViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.PUT_APPOINTMENT, model.Id);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }
        public async Task<ResponseViewModel<AppointmentViewModel>> Resheduled(AppointmentViewModel model, string reason)
        {
            var returnResponse = new ResponseViewModel<AppointmentViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.CLOSE_APPOINTMENT, model.StatusType,null);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<string>> FileUpload(HttpRequestBase httpRequestBase)
        {
            var returnResponse = new ResponseViewModel<string>();
            try
            {
                var form = new MultipartFormDataContent();
                var file = httpRequestBase.Files[0];
                var stream = file.InputStream;
                var content = new StreamContent(stream);
                content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = file.FileName
                };
                form.Add(content);

                var url = String.Format(AppointmentUserService.POST_UPLOAD_DOCUMENT);
                var response = await this.appointmentUserService.httpClient.PostAsync(url,form);
                returnResponse = await base.GetHttpResponse<string>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = "";
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }
    }
}
