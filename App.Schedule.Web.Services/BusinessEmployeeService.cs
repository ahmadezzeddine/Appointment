using System;
using System.Text;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Services
{
    public class BusinessEmployeeService : AppointmentUserBaseService, IAppointmentUserService<BusinessEmployeeService>
    {
        public BusinessEmployeeService(string token)
        {
            base.SetUpAppointmentService(token);
        }

        public async Task<ResponseViewModel<BusinessEmployeeViewModel>> VerifyLoginCredential(string Email, string Password)
        {
            var returnResponse = new ResponseViewModel<BusinessEmployeeViewModel>();
            try
            {
                Password = HttpContext.Current.Server.UrlEncode(Password);
                var url = String.Format(AppointmentUserService.GET_BUSINESS_EMP_BYLOGINID, Email, Password);
                var response = await this.appointmentUserService.httpClient.GetAsync(url);
                returnResponse = await base.GetHttpResponse<BusinessEmployeeViewModel>(response);
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = "Reason: " + ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public async Task<ResponseViewModel<string>> VerifyAndGetAdminAccessToken(string Email, string Password)
        {
            var returnResponse = new ResponseViewModel<string>();
            try
            {
                var model = "username=emp" + Email + "&password=" + Password + "&grant_type=password";
                var content = new StringContent(model, Encoding.UTF8, "text/plain");
                var url = String.Format(AppointmentUserService.GET_ADMIN_TOKEN);
                var response = await this.appointmentUserService.httpClient.PostAsync(url, content);
                var result = await response.Content.ReadAsStringAsync();
                dynamic res = JsonConvert.DeserializeObject(result);
                if (res != null)
                {
                    try
                    {
                        returnResponse.Status = true;
                        returnResponse.Data = res.access_token;
                        returnResponse.Message = "Success";
                    }
                    catch
                    {
                        returnResponse.Status = false;
                        returnResponse.Data = null;
                        returnResponse.Message = "Please check your id and password";
                    }
                }
                else
                {
                    returnResponse.Status = false;
                    returnResponse.Data = null;
                    returnResponse.Message = "There was a problem. Please try agian later.";
                }
            }
            catch (Exception ex)
            {
                returnResponse.Data = null;
                returnResponse.Message = ex.Message.ToString();
                returnResponse.Status = false;
            }
            return returnResponse;
        }

        public Task<ResponseViewModel<BusinessEmployeeService>> Add(BusinessEmployeeService model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessEmployeeService>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessEmployeeService>> Delete(long? id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessEmployeeService>> Find(Predicate<BusinessEmployeeService> pridict)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessEmployeeService>> Get(long? id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<List<BusinessEmployeeService>>> Gets()
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<BusinessEmployeeService>> Update(BusinessEmployeeService model)
        {
            throw new NotImplementedException();
        }
    }
}
