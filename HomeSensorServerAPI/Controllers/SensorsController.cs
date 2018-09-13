using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerMvc.Models
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : Controller
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ILogger<SensorsController> _logger;

        public SensorsController(ISensorRepository sensorRepository, ILogger<SensorsController> logger)
        {
            _sensorRepository = sensorRepository;
            _logger = logger;
        }

        //api/sensors/kitchen
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet("{identifier}")]
        public IEnumerable<Sensor> Get(string identifier)
        {
            return _sensorRepository.GetWithIdentifier(identifier);
        }

        [HttpPost]
        [Route("specified")]
        [Authorize(Roles = "Sensor")]
        public async Task<IActionResult> Post([FromBody] dynamic sensorDataObject)
        {
            //this sorcery is due to dynamic object, to allow wide-range of sensor without modyfining server side logic (no models needed)

            Sensor sensor = null;
            try
            {
                sensor = new Sensor
                {
                    Identifier = sensorDataObject.identifier.Value,
                    Data = JsonConvert.SerializeObject(sensorDataObject.data),
                    TimeStamp = DateTime.Now
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Invalid sensor data received, cannot parse.");
                return BadRequest();
            }

            await _sensorRepository.PostNewData(sensor);

            return Ok();
        }
    }
}