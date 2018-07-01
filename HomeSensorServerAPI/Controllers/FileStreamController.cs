using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStreamController : ControllerBase
    {
        // POST: api/image
        [HttpPost]
        public void Post(byte[] file, string filename)
        {
            //string filePath = Path.Combine(_env.ContentRootPath, "wwwroot/images/upload", filename);
            //if (System.IO.File.Exists(filePath)) return;
            //System.IO.File.WriteAllBytes(filePath, file);
        }
    }
}