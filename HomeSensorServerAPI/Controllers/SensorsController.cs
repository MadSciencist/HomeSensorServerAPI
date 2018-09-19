using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
        public async Task<IActionResult> Get(string identifier, int? skip, int? take, string property, DateTime? from, DateTime? to)
        {
            const int defaultSkip = 0;
            const int defaultTake = 200;
            var dateFrom = from ?? new DateTime(2000, 1, 1);
            var dateTo = to ?? DateTime.Now;

            var node = await _nodeRepository.GetWithIdentifierAsync(identifier);

            if (node == null) return NotFound();

            if (string.IsNullOrEmpty(property) || string.Equals(property, "all"))
            {
                var sensorData = _sensorRepository.GetWithIdentifier(identifier)
                    .Where(x => x.TimeStamp >= dateFrom && x.TimeStamp <= dateTo)
                    .OrderByDescending(x => x.TimeStamp)
                    .Skip(skip ?? defaultSkip)
                    .Take(take ?? defaultTake)
                    .OrderBy(x => x.TimeStamp)
                    .ToList();

                return Ok(new
                {
                    Identifier = identifier,
                    node.RegistredProperties,
                    Data = sensorData.Select(d => new { val = d.Data, timeStamp = d.TimeStamp })
                });
            }
            else if (node.RegistredProperties.Contains(property))
            {                
                var sensorData = _sensorRepository.GetWithIdentifier(identifier)
                    .Where(x => x.TimeStamp >= dateFrom && x.TimeStamp <= dateTo)
                    .OrderByDescending(x => x.TimeStamp)
                    .Skip(skip ?? defaultSkip)
                    .Take(take ?? defaultTake)
                    .OrderBy(x => x.TimeStamp)
                    .Select(x => new { x.TimeStamp, x.Data })
                    .ToList();

                var parsed = sensorData.Select(x => new { val = JObject.Parse(x.Data)[property], timeStamp = x.TimeStamp });

                return Ok(new
                {
                    Identifier = identifier,
                    Property = property,
                    Data = parsed
                });
            }
            else
            {
                return NotFound();
            }
  
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
 