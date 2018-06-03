using App.Schedule.Domains.ViewModel;
using System;
using App.Schedule.Web.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace App.Schedule.Web.Helpers
{
    public class BusinessHourHelper
    {
        protected string Token;
        protected long ServiceLocationId;

        public BusinessHourHelper(string _token, long _serviceLocationId)
        {
            this.Token = _token;
            this.ServiceLocationId = _serviceLocationId;
        }

        public async Task<Dictionary<int, string>> GetHoursOfDay(int day)
        {
            var getBusinessHours =await this.GetBusinessHour();
            var businessHours = getBusinessHours.SingleOrDefault(d => d.WeekDayId == (int)day);
            var hours = new Dictionary<int, string>();
            try
            {
                if (businessHours != null)
                {
                    if (!businessHours.IsHoliday)
                    {

                        var now = DateTime.Now;
                        var dateFrom = new DateTime(now.Year, now.Month, now.Day, businessHours.From.Hour, businessHours.From.Minute, businessHours.From.Second);
                        var dateTo = new DateTime(now.Year, now.Month, now.Day, businessHours.To.Hour, businessHours.To.Minute, businessHours.To.Second);
                        var hourFrom = int.Parse(dateFrom.ToString("HH"));
                        var hourTo = int.Parse(dateTo.ToString("HH"));
                        var index = 0;
                        for (var i = hourFrom; i <= hourTo; i++)
                        {
                            hours.Add(index, dateFrom.ToString("hh:mm tt"));
                            dateFrom = dateFrom.AddMinutes(30);
                            index += 1;
                            hours.Add(index, dateFrom.ToString("hh:mm tt"));
                            dateFrom = dateFrom.AddMinutes(30);
                            index += 1;
                        }

                        if (businessHours.IsSplit1.Value)
                        {
                            dateFrom = new DateTime(now.Year, now.Month, now.Day, businessHours.FromSplit1.Value.Hour, businessHours.FromSplit1.Value.Minute, businessHours.FromSplit1.Value.Second);
                            dateTo = new DateTime(now.Year, now.Month, now.Day, businessHours.ToSplit1.Value.Hour, businessHours.ToSplit1.Value.Minute, businessHours.ToSplit1.Value.Second);
                            hourFrom = int.Parse(dateFrom.ToString("HH"));
                            hourTo = int.Parse(dateTo.ToString("HH"));
                            for (var i = hourFrom; i <= hourTo; i++)
                            {
                                hours.Add(index, dateFrom.ToString("hh:mm tt"));
                                dateFrom = dateFrom.AddMinutes(30);
                                index += 1;
                                hours.Add(index, dateFrom.ToString("hh:mm tt"));
                                dateFrom = dateFrom.AddMinutes(30);
                                index += 1;
                            }
                        }

                        if (businessHours.IsSplit2.Value)
                        {
                            dateFrom = new DateTime(now.Year, now.Month, now.Day, businessHours.FromSplit2.Value.Hour, businessHours.FromSplit2.Value.Minute, businessHours.FromSplit2.Value.Second);
                            dateTo = new DateTime(now.Year, now.Month, now.Day, businessHours.ToSplit2.Value.Hour, businessHours.ToSplit2.Value.Minute, businessHours.ToSplit2.Value.Second);
                            hourFrom = int.Parse(dateFrom.ToString("HH"));
                            hourTo = int.Parse(dateTo.ToString("HH"));
                            for (var i = hourFrom; i <= hourTo; i++)
                            {
                                hours.Add(index, dateFrom.ToString("hh:mm tt"));
                                dateFrom = dateFrom.AddMinutes(30);
                                index += 1;
                                hours.Add(index, dateFrom.ToString("hh:mm tt"));
                                dateFrom = dateFrom.AddMinutes(30);
                                index += 1;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return hours;
        }

        private async Task<IEnumerable<BusinessHourViewModel>> GetBusinessHour()
        {
            var businessHours = new List<BusinessHourViewModel>();
            var BusinessHourService = new BusinessHourService(this.Token);
            var businessHourResponse = await BusinessHourService.Gets(this.ServiceLocationId, TableType.ServiceLocationId);
            if (businessHourResponse != null && businessHourResponse.Status)
            {
                return businessHourResponse.Data;
            }
            return businessHours;
        }
    }
}