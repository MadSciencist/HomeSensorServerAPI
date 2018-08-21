using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;

namespace HomeSensorServerAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NodesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Nodes
        [HttpGet]
        public IEnumerable<Node> GetAllNodes()
        {
            return _context.Nodes;
        }

        //GET: /api/nodes/type/1
        [HttpGet("type/{type}")]
        public IActionResult GetSpecifiedTypeNode(ENodeType type)
        {
            var node = _context.Nodes.Where(n => n.NodeType == type);

            if (node == null || (!node.Any()))
            {
                return NotFound();
            }

            return Ok(node);
        }

        // GET: api/Nodes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNode([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var node = await _context.Nodes.SingleOrDefaultAsync(m => m.Id == id);

            if (node == null)
            {
                return NotFound();
            }

            return Ok(node);
        }

        //TODO update
        // PUT: api/Nodes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNode([FromRoute] int id, [FromBody] Node node)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != node.Id)
            {
                return BadRequest();
            }

            IPAddress sensorIP = null, sensorGatewayIP = null;
            
            //validate IPs only for actuators - we do not carry about sensor IP, cause they might have non-static IP and thats fine
            if (node.NodeType == ENodeType.Actuator)
            {
                try
                {
                    sensorIP = IPAddress.Parse(node.IpAddress);
                    sensorGatewayIP = IPAddress.Parse(node.GatewayAddress);

                    //actuator IP must be unique
                    if(IsIPAddressUnique(sensorIP))
                    {
                        return BadRequest("Podany adres IP urządzenia już jest na liście. Adres powinien być unikalny.");
                    }
                }
                catch (FormatException)
                {
                    return BadRequest("Podany adres IP nie jest prawidłowy");
                }
            }
            else if(node.NodeType == ENodeType.Sensor)
            {
                node.IpAddress = "-";
                node.GatewayAddress = "-";
            }

            _context.Entry(node).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return BadRequest();
                }
            }catch (DbUpdateException)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Nodes
        [HttpPost]
        public async Task<IActionResult> PostNode([FromBody] Node node)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_context.Nodes.Any(n => n.Identifier == node.Identifier))
            {
                return BadRequest("Identyfikator nie jest unikalny.");
            }

            IPAddress sensorIP = null, sensorGatewayIP = null;

            //validate IPs only for actuators - we do not carry about sensor IP, cause they might have non-static IP and thats fine
            if (node.NodeType == ENodeType.Actuator)
            {
                try
                {
                    sensorIP = IPAddress.Parse(node.IpAddress);
                    sensorGatewayIP = IPAddress.Parse(node.GatewayAddress);

                    //actuator IP must be unique
                    if (IsIPAddressUnique(sensorIP))
                    {
                        return BadRequest("Podany adres IP urządzenia już jest na liście. Adres powinien być unikalny.");
                    }
                }
                catch (FormatException)
                {
                    return BadRequest("Podany adres IP nie jest prawidłowy");
                }
            }

            var verifiedNode = new Node()
            {
                Name = node.Name,
                Identifier = node.Identifier,
                Description = node.Description,
                NodeType = node.NodeType,
                SensorType = node.SensorType,
                ActuatorType = node.ActuatorType,
                IpAddress = sensorIP == null ? "-" : sensorIP.ToString(),
                GatewayAddress = sensorGatewayIP == null ? "-" : sensorGatewayIP.ToString(),
                LoginName = node.LoginName,
                LoginPassword = node.LoginPassword
            };

            _context.Nodes.Add(verifiedNode);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNode", new { id = node.Id }, node);
        }

        // DELETE: api/Nodes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNode([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var node = await _context.Nodes.SingleOrDefaultAsync(m => m.Id == id);
            if (node == null)
            {
                return NotFound();
            }

            _context.Nodes.Remove(node);
            await _context.SaveChangesAsync();

            return Ok(node);
        }

        //returns true if it is unique
        private bool IsIPAddressUnique(IPAddress IPToCompare)
            => _context.Nodes.Where(n => n.IpAddress != "-").Any(n => IPAddress.Parse(n.IpAddress).Equals(IPToCompare)) ? true : false;

        private bool NodeExists(int id) => _context.Nodes.Any(e => e.Id == id);
    }
}