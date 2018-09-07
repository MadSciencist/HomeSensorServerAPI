using HomeSensorServerAPI.Exceptions;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository.Nodes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class NodesController : ControllerBase
    {
        private readonly INodeRepository _nodeRepository;

        public NodesController(INodeRepository nodeRepository)
        {
            _nodeRepository = nodeRepository;
        }

        // GET: api/Nodes
        [HttpGet]
        public IEnumerable<Node> GetAllNodes()
        {
            return _nodeRepository.GetAll();
        }


        //GET: /api/nodes/type/1
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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNode(int id)
        {
            var node = await _nodeRepository.GetByIdAsync(id);

            if (node == null)
                return NotFound();

            return Ok(node);
        }

        //TODO update
        // PUT: api/Nodes/5
        [HttpPut("{id}")]
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

            return Ok(new { Action = "Update", updatedNode });
        }

        // POST: api/Nodes
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