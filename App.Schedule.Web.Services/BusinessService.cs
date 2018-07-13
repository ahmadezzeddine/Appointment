using System;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Services
{
    public class BusinessService : AppointmentUserBaseService, IAppointmentUserService<BusinessServiceViewModel>
    {
        public BusinessService(string token)
        {
            this.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<BusinessServiceViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<BusinessServiceViewModel>()
            {
                Status = false,
                Message = "",
                Data = new BusinessServiceViewModel()
            };
            try
            {
                var url = String.Format(AppointmentUserService.GET_BUSINESSSERVICEBYID, id);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                var result = await base.GetHttpResponse<BusinessServiceViewModel>(response);

                returnResponse.Status = result.Status;
                returnResponse.Message = result.Message;
                returnResponse.Data = result.Data;
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<BusinessServiceViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<BusinessServiceViewModel>>();
            try
            {
                var url = String.Format(AppointmentUserService.GET_BUSINESSOFFER);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<BusinessServiceViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<BusinessServiceViewModel>>> Gets(long? empId)
        {
            var returnResponse = new ResponseViewModel<List<BusinessServiceViewModel>>();
            try
            {
                if (empId.HasValue)
                {
                    var url = String.Format(AppointmentUserService.GETS_BUSINESSSERVICEBYTYPEID,empId.Value, TableType.EmployeeId);
                    var response = await this.appointmentUserService.httpClient.GetAsync(url);
                    returnResponse = await base.GetHttpResponse<List<BusinessServiceViewModel>>(response);
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

        public async Task<ResponseViewModel<List<BusinessServiceViewModel>>> Gets(long? empId, int type)
        {
            var returnResponse = new ResponseViewModel<List<BusinessServiceViewModel>>();
            try
            {
                if (empId.HasValue)
                {
                    var url = String.Format(AppointmentUserService.GETS_BUSINESSSERVICEBYTYPEID, empId.Value, type);
                    var response = await this.appointmentUserService.httpClient.GetAsync(url);
                    returnResponse = await base.GetHttpResponse<List<BusinessServiceViewModel>>(response);
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

        public async Task<ResponseViewModel<BusinessServiceViewModel>> Add(BusinessServiceViewModel model)
        {
            var returnResponse = new ResponseViewModel<BusinessServiceViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.POST_BUSINESSSERVICE);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                returnResponse = await base.GetHttpResponse<BusinessServiceViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<BusinessServiceViewModel>> Deactive(long? id, bool status)
        {
            var returnResponse = new ResponseViewModel<BusinessServiceViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DEACTIVE_BUSINESSSERVICEBYIDANDSTATUS, id.Value, status);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<BusinessServiceViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<BusinessServiceViewModel>> Delete(long? id)
        {
            var returnResponse = new ResponseViewModel<BusinessServiceViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentUserService.DELETE_BUSINESSSERVICEBYID, id.Value);
                    var response = await this.appointmentUserService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<BusinessServiceViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<BusinessServiceViewModel>> Update(BusinessServiceViewModel model)
        {
            var returnResponse = new ResponseViewModel<BusinessServiceViewModel>();
            try
            {
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var url = String.Format(AppointmentUserService.PUT_BUSINESSSERVICEBYID, model.Id);
                var response = await this.appointmentUserService.httpClient.PutAsync(url, content);
                returnResponse = await base.GetHttpResponse<BusinessServiceViewModel>(response);
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
