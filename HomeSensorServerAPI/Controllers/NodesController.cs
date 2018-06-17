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

namespace HomeSensorServerAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Nodes")]
    public class NodesController : Controller
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

        //GET: /api/nodes/type/nodeactuator
        [HttpGet("type/{type}")]
        public IActionResult GetSpecifiedTypeNode(string type)
        {
            var node = _context.Nodes.Where(n => n.Type == type);

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
                    throw;
                }
            }

            return NoContent();
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
            if (node.Type == "nodeActuator")
            {
                try
                {
                    sensorIP = IPAddress.Parse(node.IpAddress);
                    sensorGatewayIP = IPAddress.Parse(node.GatewayAddress);
                }
                catch (FormatException)
                {
                    return BadRequest("Podany adres IP nie jest prawidłowy");
                }

                //actuator IP must be unique
                var IPs = _context.Nodes.Where(n => n.IpAddress != "-").Select(n => n.IpAddress);

                foreach (var IP in IPs)
                {
                    var tempIP = IPAddress.Parse(IP);
                    if (tempIP.Equals(sensorIP))
                    {
                        return BadRequest("Podany adres IP urządzenia już jest na liście. Adres powinien być unikalny.");
                    }
                }
            }

            var candidate = new Node()
            {
                Name = node.Name,
                Identifier = node.Identifier,
                Type = node.Type,
                ExactType = node.ExactType,
                IpAddress = sensorIP == null ? "-" : sensorIP.ToString(),
                GatewayAddress = sensorGatewayIP == null ? "-" : sensorGatewayIP.ToString()
            };

            _context.Nodes.Add(candidate);

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

        private bool NodeExists(int id)
        {
            return _context.Nodes.Any(e => e.Id == id);
        }
    }
}