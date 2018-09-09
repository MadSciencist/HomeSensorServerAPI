using HomeSensorServerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PhotoUploadController : Controller
    {
        private readonly IHostingEnvironment _environment;
        private readonly ILogger<PhotoUploadController> _logger;

        public PhotoUploadController(IHostingEnvironment env, ILogger<PhotoUploadController> logger)
        {
            _environment = env;
            _logger = logger;
        }

        [HttpPost]
        [Route("Upload")]
        [Authorize(Roles = "Admin,Manager,Viewer")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest();
            }

            var fileHelper = new FileUploadHelper();

            var uploads = Path.Combine(_environment.WebRootPath, "img", "uploads", "avatars");
            var fullPath = Path.Combine(uploads, fileHelper.GetUniqueFileName(file.FileName));

            fileHelper.EnsureFolderCreation(uploads);

            try
            {
                if (file.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error while uploading avatar photo.");
            }

            return Ok(new { url = fullPath });
        }
    }
}
