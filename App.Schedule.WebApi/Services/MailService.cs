using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Collections.Generic;

namespace App.Schedule.WebApi.Services
{
    public class MailResponse
    {
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    public class MailInformation
    {
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string PlainText { get; set; }
        public string HtmlText { get; set; }
    }

    public class MailService
    {
        public async Task<Response> SG_SendMails(MailInformation mail)
        {
            try
            {
                var Key = WebConfigurationManager.AppSettings["appointment_scheduler"];
                var client = new SendGridClient(Key);
                var from = new EmailAddress("info@devgohel.com", "Appointment Scheduler");
                var toList = new List<EmailAddress>();
                foreach (var info in mail.To)
                {
                    toList.Add(new EmailAddress(info));
                }
                var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toList, mail.Subject, mail.PlainText, mail.HtmlText);
                var response = await client.SendEmailAsync(msg);
                return response;
            }
            catch
            {
                return new Response(System.Net.HttpStatusCode.ExpectationFailed, null, null);
            }
        }

        public async Task<Response> SG_SendMail(MailInformation mail)
        {
            try
            {
                var Key = WebConfigurationManager.AppSettings["appointment_scheduler"];
                var client = new SendGridClient(Key);
                var from = new EmailAddress("info@devgohel.com", "Appointment Scheduler");
                var to = new EmailAddress(mail.To[0]);
                var msg = MailHelper.CreateSingleEmail(from, to, mail.Subject, mail.PlainText, mail.HtmlText);
                var response = await client.SendEmailAsync(msg);
                return response;
            }
            catch
            {
                return new Response(System.Net.HttpStatusCode.ExpectationFailed, null, null);
            }
        }

        public async Task<MailResponse> SendMail(MailInformation mail)
        {
            if (mail.To != null && mail.To.Count < 0)
                return new MailResponse() { Message = "Sender email id required.", Status = false };

            var response = mail.To.Count > 1 ? await this.SG_SendMails(mail) : await this.SG_SendMail(mail);
            var mailResponse = new MailResponse();
            if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
            {
                mailResponse.Status = true;
                mailResponse.Message = "Sent successfuly";
            }
            else
            {
                mailResponse.Status = false;
                mailResponse.Message = "Error: " + response.Body;
            }
            return mailResponse;
        }
    }
}