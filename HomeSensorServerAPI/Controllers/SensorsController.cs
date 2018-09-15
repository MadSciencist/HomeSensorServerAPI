using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerMvc.Models
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly INodeRepository _nodeRepository;
        private readonly ILogger<SensorsController> _logger;

        public SensorsController(ISensorRepository sensorRepository, INodeRepository nodeRepository, ILogger<SensorsController> logger)
        {
            _sensorRepository = sensorRepository;
            _nodeRepository = nodeRepository;
            _logger = logger;
        }

        //api/sensors/kitchen
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet("{identifier}")]
        public async Task<IActionResult> Get(string identifier, int? skip, int? take)
        {
            var node = await _nodeRepository.GetWithIdentifierAsync(identifier);

            if (node == null) return NotFound();

            var sensorData = _sensorRepository.GetWithIdentifier(identifier)
                .OrderBy(o => o.TimeStamp)
                .Skip(skip ?? 0)
                .Take(take ?? 1000);
            
            return Ok(new {
                Identifier = identifier,
                node.RegistredProperties,
                Data = sensorData.Select(d => new { d.Data, d.TimeStamp }).ToList()
            });
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