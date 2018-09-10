using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.BindingModels;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        private readonly ISystemSettingsRepository _settingsRepository;

        public SystemController(ISystemSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        [HttpGet]
        [Route("settings")]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetSettings()
        {
            return Ok(_settingsRepository.GetAll());
        }

        [HttpPut]
        [Route("settings/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PutSettings([FromBody] RpiCredentials credentials, [FromRoute] int id)
        {
            var settingsEntity = await _settingsRepository.GetByIdAsync(id);

            settingsEntity.RpiUrl = credentials.RpiUrl;
            settingsEntity.RpiLogin = credentials.Login;
            settingsEntity.RpiPassword = credentials.Password;

            var updatedSettings = await _settingsRepository.UpdateAsync(settingsEntity);

            return Ok(new { Action = "Updated", Data = updatedSettings });
        }
    }
}