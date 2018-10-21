using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Services
{
    public class AppointmentFeedbackService : AppointmentUserBaseService, IAppointmentUserService<AppointmentFeedbackViewModel>
    {
        public AppointmentFeedbackService(string token)
        {
            this.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<AppointmentFeedbackViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentFeedbackViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_FEEDBACK_BYID, id);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<AppointmentFeedbackViewModel>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentFeedbackViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<AppointmentFeedbackViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_FEEDBACK);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentFeedbackViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentFeedbackViewModel>>> Gets(long? id)
        {
            var returnResponse = new ResponseViewModel<List<AppointmentFeedbackViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_FEEDBACK_BYBUSINESSIDANDTYPE, id.Value, (int)TableType.AppointmentFeedback);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentFeedbackViewModel>>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentFeedbackViewModel>> Add(AppointmentFeedbackViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentFeedbackViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.POST_APPOINTMENT_FEEDBACK);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentFeedbackViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public Task<ResponseViewModel<AppointmentFeedbackViewModel>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseViewModel<AppointmentFeedbackViewModel>> Delete(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentFeedbackViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_APPOINTMENT_FEEDBACK, id.Value);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<AppointmentFeedbackViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentFeedbackViewModel>> Update(AppointmentFeedbackViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentFeedbackViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.PUT_APPOINTMENT_FEEDBACK, model.Id);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentFeedbackViewModel>(response);
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
