using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;
namespace App.Schedule.Web.Services
{
    public class AppointmentInviteeService : AppointmentUserBaseService, IAppointmentUserService<AppointmentInviteeViewModel>
    {
        public AppointmentInviteeService(string token)
        {
            base.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<AppointmentInviteeViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentInviteeViewModel>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_INVITEE_BYID, id);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<AppointmentInviteeViewModel>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentInviteeViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<AppointmentInviteeViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_INVITEE);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentInviteeViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentInviteeViewModel>>> Gets(long? id, TableType type)
        {
            var returnResponse = new ResponseViewModel<List<AppointmentInviteeViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_APPOINTMENT_INVITEE_BYID_TYPE, id.Value, (int)type);
                var response = this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentInviteeViewModel>>(response.Result);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentInviteeViewModel>> Add(AppointmentInviteeViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentInviteeViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.POST_APPOINTMENT_INVITEE);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentInviteeViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public Task<ResponseViewModel<AppointmentInviteeViewModel>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseViewModel<AppointmentInviteeViewModel>> Delete(long? id)
        {
            var returnResponse = new ResponseViewModel<AppointmentInviteeViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_APPOINTMENT_INVITEE, id.Value);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<AppointmentInviteeViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<AppointmentInviteeViewModel>> Update(AppointmentInviteeViewModel model)
        {
            var returnResponse = new ResponseViewModel<AppointmentInviteeViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.PUT_APPOINTMENT_INVITEE, model.Id);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<AppointmentInviteeViewModel>(response);
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
