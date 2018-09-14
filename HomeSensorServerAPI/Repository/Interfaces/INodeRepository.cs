using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        IEnumerable<Node> GetWithType(ENodeType type);
        Task<Node> GetWithIdentifierAsync(string identifier);
    }
}
