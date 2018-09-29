using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Schedule.Domains.ViewModel;
using App.Schedule.Web.Admin.Services;

namespace App.Schedule.Services
{
    public class BusinessService : AppointmentBaseService, IAppointmentService<BusinessViewModel>
    {
        public BusinessService(string token)
        {
            base.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<BusinessViewModel>> Get(long? id)
        {
            var returnResponse = new ResponseViewModel<BusinessViewModel>();
            try
            {
                var url = String.Format(AppointmentService.GET_BUSINESS_BYID, id.Value);
                var response = await this.appointmentService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<BusinessViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Please try again later. ex: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<BusinessViewModel>>> Gets()
        {
            var returnResponse = new ResponseViewModel<List<BusinessViewModel>>();
            try
            {
                var url = String.Format(AppointmentService.GET_BUSINESSES);
                var response = await this.appointmentService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<BusinessViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Please try again later. ex: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<BusinessEmployeeViewModel>>> GetEmployees(long? id, TableType type)
        {
            var returnResponse = new ResponseViewModel<List<BusinessEmployeeViewModel>>();
            try
            {
                var url = String.Format(AppointmentService.GET_BUSINESS_EMPLOYEES,id.Value,(int)type);
                var response = await this.appointmentService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<BusinessEmployeeViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Please try again later. ex: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<BusinessCustomerViewModel>>> GetCustomers(long? id, TableType type)
        {
            var returnResponse = new ResponseViewModel<List<BusinessCustomerViewModel>>();
            try
            {
                if (!id.HasValue)
                    return returnResponse;

                var url = String.Format(AppointmentService.GET_BUSINESS_CUSTOMERS,id.Value,(int) type);
                var response = await this.appointmentService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<BusinessCustomerViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Please try again later. ex: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<List<AppointmentViewModel>>> GetAppointmentss(long? id, TableType type)
        {
            var returnResponse = new ResponseViewModel<List<AppointmentViewModel>>();
            try
            {
                if (!id.HasValue)
                    return returnResponse;

                var url = String.Format(AppointmentService.GET_USERS_APPOINTMENTS, id.Value, (int)type);
                var response = await this.appointmentService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<List<AppointmentViewModel>>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Please try again later. ex: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public Task<ResponseViewModel<BusinessViewModel>> Add(BusinessViewModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseViewModel<BusinessViewModel>> Deactive(long? id, bool status)
        {
            var returnResponse = new ResponseViewModel<BusinessViewModel>();
            try
            {
                if (!id.HasValue)
                {
                    returnResponse.Status = false;
                    returnResponse.Message = "Please enter a valid offer id.";
                }
                else
                {
                    var url = String.Format(AppointmentService.DEACTIVE_BUSINESSRBYIDANDSTATUS, id.Value, status);
                    var response = await this.appointmentService.httpClient.DeleteAsync(url);
                    returnResponse = await base.GetHttpResponse<BusinessViewModel>(response);
                }
            }
            catch (Exception ex)
            {
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public Task<ResponseViewModel<BusinessViewModel>> Delete(long? id)
        {
            throw new NotImplementedException();
        }
        
        public Task<ResponseViewModel<BusinessViewModel>> Update(BusinessViewModel model)
        {
            throw new NotImplementedException();
        }

       
    }
}
