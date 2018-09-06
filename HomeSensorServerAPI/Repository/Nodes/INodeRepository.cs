using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository.Users
{
    public interface INodeRepository : IRepository<Node>
    {
        IEnumerable<Node> GetWithType(ENodeType type);
    }
}
