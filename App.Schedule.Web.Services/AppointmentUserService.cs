using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.Web.Services
{
    public class AppointmentUserService
    {
        public HttpClient httpClient;
        //public const string baseUrl = "http://appointment.why-fi.com/";
        public static string baseUrl = "http://localhost/appointmentapi/";
        //public static string baseUrl = "http://localhost:57433/";

        //Admin Identity Token API
        public const string POST_API_ACCOUNT_REGISTER = "api/account/register";
        public const string PUT_API_ACCOUNT = "api/account/updateuser";
        public const string DELETE_API_ACCOUNT = "api/account/deleteuser";
        public const string GET_ADMIN_TOKEN = "token";

        //Business
        public const string GET_BUSINESS_BYID = "api/business?id={0}";
        public const string PUT_BUSINESS = "api/business?id={0}";
        public const string PUT_BUSINESS_BYTPE = "api/business?id={0}&type={1}";

        //Country
        public const string GET_COUNTRIES = "api/country";
        public const string GET_COUNTRIE_BYID = "api/country?id={0}";

        //Business Category
        public const string GET_BUSINESSCATEGORIES = "api/businesscategory";

        //Timezone
        public const string GET_TIMEZONES = "api/timezone";
        
        //Membership API
        public const string GET_MEMBERSHIPS = "api/membership";
        public const string GET_MEMBERSHIP_BYID = "api/membership?id={0}";

        //Business Hour
        public const string GET_BUSINESSHOURS = "api/businesshour";
        public const string GET_BUSINESSHOURSBYId = "api/businesshour?id={0}";
        public const string GET_BUSINESSHOURSBYTYPE = "api/businesshour?id={0}&type={1}";
        public const string PUT_BUSINESSHOUR = "api/businesshour?id={0}";

        //Business Holiday
        public const string GET_BUSINESSHOLIDAY = "api/businessholiday";
        public const string GET_BUSINESSHOLIDAYBYId = "api/businessholiday?id={0}";
        public const string GET_BUSINESSHOLIDAYSBYTYPE = "api/businessholiday?id={0}&type={1}";
        public const string POST_BUSINESSHOLIDAY = "api/businessholiday";
        public const string PUT_BUSINESSHOLIDAY = "api/businessholiday?id={0}";
        public const string DELETE_BUSINESSHOLIDAYBYId = "api/businessholiday?id={0}";

        //Service Location
        public const string GET_SERVICELOCATION = "api/servicelocation";
        public const string GET_SERVICELOCATIONBYIDANDTYPE = "api/servicelocation?id={0}&type={1}";
        public const string GET_SERVICELOCATIONBYID = "api/servicelocation?id={0}";
        public const string DELETE_SERVICELOCATIONBYID = "api/servicelocation?id={0}&type={1}";
        public const string POST_SERVICELOCATION = "api/servicelocation";
        public const string PUT_SERVICELOCATION = "api/servicelocation?id={0}";

        //Business Offer
        public const string GET_BUSINESSOFFER = "api/businessoffer";
        public const string GET_BUSINESSOFFERBYID = "api/businessoffer?id={0}";
        public const string GETS_BUSINESSOFFERBYIDANDTYPE = "api/businessoffer?id={0}&type={1}";
        public const string POST_BUSINESSOFFER = "api/businessoffer";
        public const string PUT_BUSINESSOFFERBYID = "api/businessoffer?id={0}";
        public const string DELETE_BUSINESSOFFERBYID = "api/businessoffer?id={0}";
        public const string DEACTIVE_BUSINESSOFFERBYIDANDSTATUS = "api/businessoffer?id={0}&status={1}";

        //Business Offer Location
        public const string GET_BUSINESSOFFERSERVICELOCATION = "api/businessofferlocation";
        public const string GET_BUSINESSOFFERSERVICELOCATIONBYID = "api/businessofferlocation?id={0}";
        public const string GETS_BUSINESSOFFERSERVICELOCATION = "api/businessofferlocation?id={0}&type={1}";
        //public const string GETS_BUSINESSOFFERSERVICELOCATION_TTYPE = "api/businessofferlocation?id={0}&type={1}";
        public const string POST_BUSINESSOFFERSERVICELOCATION = "api/businessofferlocation";
        public const string PUT_BUSINESSOFFERSERVICELOCATION = "api/businessofferlocation?id={0}";
        public const string DELETE_BUSINESSOFFERSERVICELOCATION = "api/businessofferlocation?id={0}";
        public const string DEACTIVE_BUSINESSOFFERSERVICELOCATION = "api/businessofferlocation?id={0}&status={1}";

        //Business Service
        public const string GET_BUSINESSSERVICE = "api/businessservice";
        public const string GET_BUSINESSSERVICEBYID = "api/businessservice?id={0}";
        public const string GETS_BUSINESSSERVICEBYTYPEID = "api/businessservice?id={0}&type={1}";
        public const string POST_BUSINESSSERVICE = "api/businessservice";
        public const string PUT_BUSINESSSERVICEBYID = "api/businessservice?id={0}";
        public const string DELETE_BUSINESSSERVICEBYID = "api/businessservice?id={0}";
        public const string DEACTIVE_BUSINESSSERVICEBYIDANDSTATUS = "api/businessservice?id={0}&status={1}";

        //Business Employee
        public const string GET_BUSINESS_EMP_BYLOGINID = "api/businessemployee?email={0}&password={1}&hasForgot={2}";
        public const string GET_EMPLOYEESBYID = "api/businessemployee?id={0}";
        public const string GET_EMPLOYEESBYIDANDTYPE = "api/businessemployee?id={0}&type={1}";
        public const string POST_EMPLOYEES = "api/businessemployee";
        public const string PUT_EMPLOYEES = "api/businessemployee?id={0}";
        public const string DELETE_EMPLOYEES = "api/businessemployee?id={0}";
        public const string DEACTIVE_EMPLOYEES = "api/businessemployee?id={0}&status={1}";

        //Business Customer
        public const string GET_BUSINESS_CUSTOMER_BYLOGINID = "api/businesscustomer?email={0}&password={1}&hasForgot={2}";
        public const string GET_BUSINESS_CUSTOMERBYID = "api/businesscustomer?id={0}";
        public const string GET_BUSINESS_CUSTOMERBYIDANDTYPE = "api/businesscustomer?id={0}&type={1}";
        public const string PUT_BUSINESS_CUSTOMER = "api/businesscustomer?id={0}";
        public const string DELETE_BUSINESS_CUSTOMER= "api/businesscustomer?id={0}";
        public const string DEACTIVE_BUSINESS_CUSTOMER = "api/businesscustomer?id={0}&status={1}";

        //Appointment
        public const string GET_APPOINTMENT  = "api/appointment";
        public const string GET_APPOINTMENT_BYID = "api/appointment?id={0}";
        public const string GET_APPOINTMENT_BYBUSINESSIDANDTYPE = "api/appointment?id={0}&type={1}";
        public const string GET_APPOINTMENT_PAYMENT_BY_ID = "api/appointmentpayment?id={0}";
        public const string POST_APPOINTMENT = "api/appointment";
        public const string PUT_APPOINTMENT = "api/appointment?id={0}";
        public const string DELETE_APPOINTMENT = "api/appointment?id={0}";
        public const string DEACTIVE_APPOINTMENT = "api/appointment?id={0}&status={1}";
        public const string CLOSE_APPOINTMENT = "api/appointment?type={0}&reason={1}";
        public const string CANCEL_APPOINTMENT = "api/appointment?id={0}&type={1}&reason={2}";

        //Document Category
        public const string GET_DOCUMENTCATEGORIES = "api/documentcategory";
        public const string POST_DOCUMENTCATEGORY = "api/documentcategory";
        public const string GET_DOCUMENTCATEGORY_BYID = "api/documentcategory?id={0}";
        public const string PUT_DOCUMENTCATEGORY_BYID = "api/documentcategory?id={0}";
        public const string PUT_DOCUMENTCATEGORY = "api/documentcategory?id={0}&status={1}&type={2}";
        public const string DELETE_DOCUMENTCATEGORY = "api/documentcategory?id={0}&status={1}&type={2}";

        //Appointment Document
        public const string GET_APPOINTMENT_DOCUMENT = "api/appointmentdocument";
        public const string GET_APPOINTMENT_DOCUMENT_BYID = "api/appointmentdocument?id={0}";
        public const string GET_APPOINTMENT_DOCUMENT_BYBUSINESSIDANDTYPE = "api/appointmentdocument?id={0}&type={1}";
        public const string POST_APPOINTMENT_DOCUMENT = "api/appointmentdocument";
        public const string PUT_APPOINTMENT_DOCUMENT = "api/appointmentdocument?id={0}";
        public const string DELETE_APPOINTMENT_DOCUMENT = "api/appointmentdocument?id={0}";
        public const string DEACTIVE_APPOINTMENT_DOCUMENT = "api/appointmentdocument?id={0}&status={1}";
        public const string CLOSE_APPOINTMENT_DOCUMENT = "api/appointmentdocument?type={0}&reason={1}";

        //Appointment Feedback
        public const string GET_APPOINTMENT_FEEDBACK = "api/appointmentfeedback";
        public const string GET_APPOINTMENT_FEEDBACK_BYID = "api/appointmentfeedback?id={0}";
        public const string GET_APPOINTMENT_FEEDBACK_BYBUSINESSIDANDTYPE = "api/appointmentfeedback?id={0}&type={1}";
        public const string POST_APPOINTMENT_FEEDBACK = "api/appointmentfeedback";
        public const string PUT_APPOINTMENT_FEEDBACK = "api/appointmentfeedback?id={0}";
        public const string DELETE_APPOINTMENT_FEEDBACK = "api/appointmentfeedback?id={0}";
        public const string DEACTIVE_APPOINTMENT_FEEDBACK = "api/appointmentfeedback?id={0}&status={1}";
        public const string CLOSE_APPOINTMENT_FEEDBACK = "api/appointmentfeedback?type={0}&reason={1}";

        //Appointment Invitee
        public const string GET_APPOINTMENT_INVITEE = "api/appointmentinvitee";
        public const string GET_APPOINTMENT_INVITEE_BYID = "api/appointmentinvitee?id={0}";
        public const string GET_APPOINTMENT_INVITEE_BYID_TYPE = "api/appointmentinvitee?id={0}&type={1}";
        public const string POST_APPOINTMENT_INVITEE = "api/appointmentinvitee";
        public const string PUT_APPOINTMENT_INVITEE = "api/appointmentinvitee?id={0}";
        public const string DELETE_APPOINTMENT_INVITEE = "api/appointmentinvitee?id={0}";

        //Upload
        public const string POST_UPLOAD_DOCUMENT = "api/upload";


        public AppointmentUserService()
        {
            this.httpClient = new HttpClient() { BaseAddress = new Uri(baseUrl) };
            this.httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }
    }

    public class AppointmentUserBaseService
    {
        protected AppointmentUserService appointmentUserService;

        protected void SetUpAppointmentService(string token)
        {
            appointmentUserService = new AppointmentUserService();
            this.appointmentUserService.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ResponseViewModel<T>> GetHttpResponse<T>(HttpResponseMessage response)
        {
            var returnResponse = new ResponseViewModel<T>();
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    dynamic res = JsonConvert.DeserializeObject<ResponseViewModel<T>>(result, settings);
                    if (res != null)
                    {
                        returnResponse.Status = res.Status;
                        returnResponse.Message = res.Message;
                        returnResponse.Data = res.Data;
                    }
                    else
                    {
                        returnResponse.Status = false;
                        returnResponse.Message = "There was a problem. Please try again later.";
                    }
                }
                else
                {
                    returnResponse.Status = false;
                    returnResponse.Message = response.ReasonPhrase;
                }
            }
            catch (Exception ex)
            {
                returnResponse.Status = false;
                returnResponse.Message = ex.Message.ToString();
            }
            return returnResponse;
        }

        public async Task<List<T>> GetHttpResponseList<T>(HttpResponseMessage response)
        {
            var model = new List<T>();
            try
            {
                var result = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<ResponseViewModel<List<T>>>(result);
                if (res != null)
                    if (res.Status)
                        model = res.Data;
            }
            catch
            {
                model = null;
            }
            return model;
        }
    }

    
}
