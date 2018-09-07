using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository.Generic;
using System.Collections.Generic;

namespace HomeSensorServerAPI.Repository.Nodes
{
    public interface INodeRepository : IGenericRepository<Node>
    {
        IEnumerable<Node> GetWithType(ENodeType type);
    }
}
