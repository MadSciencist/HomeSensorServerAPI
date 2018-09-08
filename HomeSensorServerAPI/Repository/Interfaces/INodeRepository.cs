using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using System.Collections.Generic;

namespace HomeSensorServerAPI.Repository
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        IEnumerable<Node> GetWithType(ENodeType type);
    }
}
