using App.Schedule.Domains.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public Task<ResponseViewModel<AppointmentViewModel>> Find(Predicate<AppointmentViewModel> pridict)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<AppointmentViewModel>> Get(long? id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<List<AppointmentViewModel>>> Gets()
        {
            throw new NotImplementedException();
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

        public Task<ResponseViewModel<AppointmentViewModel>> Add(AppointmentViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<AppointmentViewModel>> Deactive(long? id, bool status)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<AppointmentViewModel>> Delete(long? id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseViewModel<AppointmentViewModel>> Update(AppointmentViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
