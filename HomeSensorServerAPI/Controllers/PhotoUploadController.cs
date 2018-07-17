using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoUploadController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment _environment;

        public PhotoUploadController(AppDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        [HttpPost]
        [Route("test")]
        public IActionResult A()
        {
            return Ok("asd");
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            if(file == null)
            {
                return BadRequest();
            }

            var uploads = Path.Combine(_environment.WebRootPath, "img", "uploads", "avatars");
            var fullPath = Path.Combine(uploads, GetUniqueFileName(file.FileName));

            EnsureFolderCreation(uploads);

            if (file.Length > 0)
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return Ok(new { url = fullPath });
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        private void EnsureFolderCreation(string path)
        {
            if ((path.Length > 0) && (!Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
