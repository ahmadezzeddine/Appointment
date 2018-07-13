using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using App.Schedule.Context;
using App.Schedule.Domains.ViewModel;

namespace App.Schedule.WebApi.Controllers
{
    [AllowAnonymous]
    public class FileController : ApiController
    {
        private readonly AppScheduleDbContext _db;

        public FileController()
        {
            _db = new AppScheduleDbContext();
        }

        // GET: api/Appointment
        [HttpGet]
        [AllowAnonymous]
        public HttpResponseMessage Get(string id, TableType type)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    //var Id_Value = Security.Decrypt(id, true);
                    int _id = 0;
                    if (int.TryParse(id, out _id))
                    {
                        if (type == TableType.Document)
                        {
                            var document = _db.tblAppointmentDocuments.Find(_id);
                            if (document != null)
                            {
                                var file = document.DocumentLink;
                                var path = System.Web.Hosting.HostingEnvironment.MapPath(document.DocumentLink);
                                var fileBytes = File.ReadAllBytes(path);
                                Stream bytesToStream = new MemoryStream(fileBytes);
                                var response = Request.CreateResponse(HttpStatusCode.OK);
                                response.Content = new StreamContent(bytesToStream);
                                string mimeType = MimeMapping.GetMimeMapping(document.DocumentLink);
                                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                                return response;
                            }
                        }
                    }
                }
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
