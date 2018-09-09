using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ServerMvc.Models
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SensorsController : Controller
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorsController(ISensorRepository sensorRepository)
        {
            _sensorRepository = sensorRepository;
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
        public void Post([FromBody]Sensor sensor)
        {
            if (sensor != null && ModelState.IsValid)
            {
                _sensorRepository.PostNewData(sensor);
            }
        }
    }
}