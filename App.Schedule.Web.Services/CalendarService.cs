using App.Schedule.Domains.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Schedule.Web.Services
{
    public class CalendarService : AppointmentUserBaseService, IAppointmentUserService<AppointmentViewModel>
    {
        public CalendarService(string token)
        {
            this.SetUpAppointmentService(token);
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

        public Task<ResponseViewModel<AppointmentViewModel>> Update(AppointmentViewModel model)
        {
            throw new NotImplementedException();
        }
    }
}
