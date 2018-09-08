using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ServerMvc.Models
{
    // [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorsController(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
        }

        //api/sensors/kitchen
        [HttpGet("{identifier}")]
        public IEnumerable<Sensor> Get(string identifier)
        {
            return _sensorRepository.GetWithIdentifier(identifier);
        }

        [HttpPost]
        [Route("specified")]
        public void Post([FromBody]Sensor sensor)
        {
            if (sensor != null && ModelState.IsValid)
            {
                _sensorRepository.PostNewData(sensor);
            }
        }
    }
}