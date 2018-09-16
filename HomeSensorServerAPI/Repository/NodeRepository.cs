using HomeSensorServerAPI.Exceptions;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository
{
    public class NodeRepository : GenericRepository<Node>, INodeRepository
    {
        private readonly ILogger _logger;

        public NodeRepository(AppDbContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory) {
            _logger = loggerFactory.CreateLogger(typeof(NodeRepository));
        }

        public IEnumerable<Node> GetWithType(ENodeType type)
        {
            return _context.Nodes.Where(n => n.NodeType == type);
        }
        public async Task<Node> GetWithIdentifierAsync(string identifier)
        {
            var node = await _context.Nodes.FirstOrDefaultAsync(n => n.Identifier == identifier);

            if (node == null)
                _logger.LogWarning($"Tried to search for non existing identifier: {identifier}");

            return node;
        }

        public override async Task<Node> CreateAsync(Node node)
        {
            IPAddress nodeIP = null, sensorGatewayIP = null;

            if (_context.Nodes.Any(n => n.Identifier == node.Identifier))
            {
                _logger.LogWarning("Create node failed: Identifier is not unique.");
                throw new IdentifierNotUniqueException("Identifier is not unique.");
            }

            //validate IPs only for actuators - we do not carry about sensor IP, cause they might have non-static IP and thats fine
            if (node.NodeType == ENodeType.Actuator)
            {
                try
                {
                    nodeIP = IPAddress.Parse(node.IpAddress);
                    sensorGatewayIP = IPAddress.Parse(node.GatewayAddress);

                    //actuator IP must be unique
                    if (!IsIpAddressUnique(nodeIP))
                    {
                        _logger.LogWarning("Create node failed: IP address is not unique.");
                        throw new IpAddressNotUniqueException("IP address is already on the list.");
                    }
                }
                catch (FormatException)
                {
                    _logger.LogWarning("Create node failed: IP address is not valid.");
                    throw new FormatException("The IP address is not valid.");
                }
            }

            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            return node;
        }

        public override async Task<Node> UpdateAsync(Node node)
        {
            var existingNode = await _context.Nodes.AsNoTracking().SingleOrDefaultAsync(n => n.Id == node.Id);
            IPAddress sensorIP = null, sensorGatewayIP = null;

            //check if there already exissts node with that identifier, if yes, then if it's identifier is other than new one 
            bool isUnique = IsIdentifierUnique(node.Identifier);
            bool isEditingHisself = existingNode.Identifier == node.Identifier;
            if (existingNode != null && (!isUnique && !isEditingHisself))
            {
                _logger.LogWarning("Update node failed: Identifier is not unique.");
                throw new IdentifierNotUniqueException("Identifier is not unique.");
            }

            //validate IPs only for actuators - we do not carry about sensor IP, cause they might have non-static IP and thats fine
            if (node.NodeType == ENodeType.Actuator)
            {
                try
                {
                    sensorIP = IPAddress.Parse(node.IpAddress);
                    sensorGatewayIP = IPAddress.Parse(node.GatewayAddress);

                    //actuator IP must be unique
                    if (!IsIpAddressUnique(sensorIP) && existingNode.IpAddress != node.IpAddress)
                    {
                        _logger.LogWarning("Update node failed: IP adress is not unique.");
                        throw new IpAddressNotUniqueException("IP address is already on the list.");
                    }
                }
                catch (FormatException)
                {
                    _logger.LogWarning("Update node failed: IP address is not valid.");
                    throw new FormatException("The IP address is not valid.");
                }
            }
            else if (node.NodeType == ENodeType.Sensor)
            {
                node.IpAddress = String.Empty;
                node.GatewayAddress = String.Empty;
            }

            _context.Entry(node).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError(e, "Exception while updating node entity.");
            }

            return node;
        }

        //returns true if it is unique
        private bool IsIpAddressUnique(IPAddress IPToCompare)
            => _context.Nodes.Where(n => n.NodeType == ENodeType.Actuator).Any(n => IPAddress.Parse(n.IpAddress).Equals(IPToCompare)) ? false : true;

        //returns true if it is unique
        private bool IsIdentifierUnique(string identifier)
            => _context.Nodes.Any(n => n.Identifier == identifier) ? false : true;
    }
}
