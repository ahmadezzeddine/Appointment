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

        public Task<ResponseViewModel<BusinessViewModel>> Add(BusinessViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessViewModel>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessViewModel>> Delete(long? id)
        {
            throw new NotImplementedException();
        }
        
        public Task<ResponseViewModel<BusinessViewModel>> Update(BusinessViewModel model)
        {
            throw new NotImplementedException();
        }

        Task<ResponseViewModel<BusinessViewModel>> IAppointmentService<BusinessViewModel>.Get(long? id)
        {
            throw new NotImplementedException();
        }
    }
}
