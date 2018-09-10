using HomeSensorServerAPI.Exceptions;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NodesController : Controller
    {
        private readonly INodeRepository _nodeRepository;

        public NodesController(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        // GET: api/Nodes

        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet]
        public IEnumerable<Node> GetAllNodes()
        {
            return _nodeRepository.GetAll();
        }


        //GET: /api/nodes/type/1
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet("type/{type}")]
        public IActionResult GetSpecifiedTypeNode(ENodeType type)
        {
            var specifiedTypeNodes = _nodeRepository.GetWithType(type);

            if (specifiedTypeNodes == null || (!specifiedTypeNodes.Any()))
            {
                return NotFound();
            }

            return Ok(specifiedTypeNodes);
        }

        // GET: api/Nodes/5
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNode(int id)
        {
            var node = await _nodeRepository.GetByIdAsync(id);

            if (node == null)
                return NotFound();

            return Ok(node);
        }

        // PUT: api/Nodes/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PutNode([FromRoute] int id, [FromBody] Node node)
        {
            Node updatedNode = null;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != node.Id)
                return BadRequest();

            try
            {
                updatedNode = await _nodeRepository.UpdateAsync(node);
            }
            catch (IdentifierNotUniqueException e)
            {
                return BadRequest("Identyfikator nie jest unikalny");
            }
            catch (IpAddressNotUniqueException e)
            {
                return BadRequest("Adres IP nie jest unikalny");
            }
            catch (FormatException e)
            {
                return BadRequest("Adres IP nie jest prawidłowy.");
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(new { Action = "Update", updatedNode });
        }

        // POST: api/Nodes
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> PostNode([FromBody] Node node)
        {
            Node createdNode = null;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                createdNode = await _nodeRepository.CreateAsync(node);
            }
            catch (IdentifierNotUniqueException)
            {
                return BadRequest("Identyfikator nie jest unikalny");
            }
            catch (IpAddressNotUniqueException)
            {
                return BadRequest("Adres IP nie jest unikalny");
            }
            catch (FormatException)
            {
                return BadRequest("Adres IP nie jest prawidłowy.");
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return CreatedAtAction("PostNode", createdNode);
        }

        // DELETE: api/Nodes/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNode(int id)
        {
            var node = await _nodeRepository.GetByIdAsync(id);

            if (node == null)
            {
                return NotFound();
            }

            await _nodeRepository.DeleteAsync(node);

            return Ok(new { Action = "Deleted", node});
        }
    }
}