using System;
using System.Web;
using System.Web.Http;

namespace App.Schedule.WebApi.Controllers
{
    public class UploadController : ApiController
    {
        [System.Web.Http.HttpPost]
        public IHttpActionResult Post()
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var postedFile = httpRequest.Files[0];
                var fileName = "~/Content/Files/" + Guid.NewGuid() + postedFile.FileName;
                var filePath = HttpContext.Current.Server.MapPath(fileName);
                postedFile.SaveAs(filePath);
                return Ok(new { status = true, data = fileName, message = "success" });
            }
            else
            {
                return Ok(new { status = false, data = "", message = "failed" });
            }
        }
    }
}
