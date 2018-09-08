using HomeSensorServerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository
{
    public interface ISensorRepository: IGenericRepository<Sensor>
    {
        IEnumerable<Sensor> GetWithIdentifier(string identifier);
        Task PostNewData(Sensor sensor);
    }
}
