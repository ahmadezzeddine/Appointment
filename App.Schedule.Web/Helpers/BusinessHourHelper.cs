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
                        var hourFrom = new TimeSpan(dateFrom.Hour,dateFrom.Minute,dateFrom.Second).TotalMinutes;
                        var hourTo = new TimeSpan(dateTo.Hour, dateTo.Minute, dateTo.Second).TotalMinutes;
                        var index = 0;
                        for (var i = hourFrom; i <= hourTo; i+=30)
                        {
                            hours.Add(index, dateFrom.ToString("hh:mm tt"));
                            dateFrom = dateFrom.AddMinutes(30);
                            index += 1;
                        }

                        if (businessHours.IsSplit1.Value)
                        {
                            var dateFrom1 = new DateTime(now.Year, now.Month, now.Day, businessHours.FromSplit1.Value.Hour, businessHours.FromSplit1.Value.Minute, businessHours.FromSplit1.Value.Second);
                            var dateTo1 = new DateTime(now.Year, now.Month, now.Day, businessHours.ToSplit1.Value.Hour, businessHours.ToSplit1.Value.Minute, businessHours.ToSplit1.Value.Second);
                            var hourFrom1 = new TimeSpan(dateFrom1.Hour, dateFrom1.Minute, dateFrom1.Second).TotalMinutes;
                            var hourTo1 = new TimeSpan(dateTo1.Hour, dateTo1.Minute, dateTo1.Second).TotalMinutes;
                            for (var i = hourFrom1; i <= hourTo1; i+=30)
                            {
                                hours.Add(index, dateFrom1.ToString("hh:mm tt"));
                                dateFrom1 = dateFrom1.AddMinutes(30);
                                index += 1;
                            }
                        }

                        if (businessHours.IsSplit2.Value)
                        {
                            var dateFrom2 = new DateTime(now.Year, now.Month, now.Day, businessHours.FromSplit2.Value.Hour, businessHours.FromSplit2.Value.Minute, businessHours.FromSplit2.Value.Second);
                            var dateTo2 = new DateTime(now.Year, now.Month, now.Day, businessHours.ToSplit2.Value.Hour, businessHours.ToSplit2.Value.Minute, businessHours.ToSplit2.Value.Second);
                            var hourFrom2 = new TimeSpan(dateFrom2.Hour, dateFrom2.Minute, dateFrom2.Second).TotalMinutes;
                            var hourTo2 = new TimeSpan(dateTo2.Hour, dateTo2.Minute, dateTo2.Second).TotalMinutes;
                            for (var i = hourFrom2; i <= hourTo2; i+=30)
                            {
                                hours.Add(index, dateFrom2.ToString("hh:mm tt"));
                                dateFrom2 = dateFrom2.AddMinutes(30);
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