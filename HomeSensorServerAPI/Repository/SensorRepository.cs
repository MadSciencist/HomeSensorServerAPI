using HomeSensorServerAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository
{
    public class SensorRepository : GenericRepository<Sensor>, ISensorRepository
    {
        private readonly ILogger _logger;

        public SensorRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(typeof(SensorRepository));
        }
        public IEnumerable<Sensor> GetWithIdentifier(string identifier)
        {
            return _context.Sensors.Where(s => s.Identifier == identifier).ToList();
        }
        
        public async Task PostNewData(Sensor sensor)
        {
            try
            {
                if (_context.Nodes.Any(n => n.Identifier == sensor.Identifier))
                {
                    var actualCount = _context.Sensors.Count(s => s.Identifier == sensor.Identifier);

                    if (actualCount >= 1000) //keep only last 1000 samples
                    {
                        var toRemove = _context.Sensors.Where(s => s.Identifier == sensor.Identifier).OrderBy(s => s.ID).Take(1);
                        _context.Sensors.RemoveRange(toRemove);
                    }
                    _context.Sensors.Add(sensor);

                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError(e, "Error while updading entity - concurrency");
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Error while updading entity");
            }
        }
    }
}
