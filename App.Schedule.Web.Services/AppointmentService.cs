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

        public Task<ResponseViewModel<AppointmentViewModel>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<AppointmentViewModel>> Delete(long? id)
        {
            throw new NotImplementedException();
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
    }
}
