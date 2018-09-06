using HomeSensorServerAPI.Exceptions;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Repository.Users
{
    public class NodeRepository : Repository<Node>, INodeRepository
    {
        public NodeRepository(AppDbContext context) : base(context) { }

        public IEnumerable<Node> GetWithType(ENodeType type)
        {
            return _context.Nodes.Where(n => n.NodeType == type);
        }

        public override async Task<Node> CreateAsync(Node node)
        {
            IPAddress nodeIP = null, sensorGatewayIP = null;

            if (_context.Nodes.Any(n => n.Identifier == node.Identifier))
            {
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
                        throw new IpAddressNotUniqueException("IP address is already on the list");
                    }
                }
                catch (FormatException)
                {
                    throw new FormatException("The IP address is not valid.");
                }
            }

            _context.Nodes.Add(node);
            await _context.SaveChangesAsync();

            return node;
        }

        public override async Task<Node> UpdateAsync(Node node)
        {
            var existingNode = _context.Nodes.FirstOrDefault(n => n.Id == node.Id);
            IPAddress sensorIP = null, sensorGatewayIP = null;
            _context.Entry(node).State = EntityState.Detached;

            //check if there already exissts node with that identifier, if yes, then if it's identifier is other than new one 
            if (existingNode != null && existingNode.Identifier != node.Identifier)
            {
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
                        throw new IpAddressNotUniqueException("IP address is already on the list");
                    }
                }
                catch (FormatException)
                {
                    throw new FormatException("The IP address is not valid.");
                }
            }
            else if (node.NodeType == ENodeType.Sensor)
            {
                node.IpAddress = "";
                node.GatewayAddress = "";
            }

            _context.Entry(node).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
            }


            return node;
        }

        //returns true if it is unique
        private bool IsIpAddressUnique(IPAddress IPToCompare)
            => _context.Nodes.Where(n => n.IpAddress != "-").Any(n => IPAddress.Parse(n.IpAddress).Equals(IPToCompare)) ? false : true;
    }
}
