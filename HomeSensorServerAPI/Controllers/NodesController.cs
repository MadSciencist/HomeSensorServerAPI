using HomeSensorServerAPI.Exceptions;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Utils;
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
    public class NodesController : ControllerBase
    {
        private readonly INodeRepository _nodeRepository;
        private readonly IUserRepository _userRepository;

        public NodesController(INodeRepository nodeRepository, IUserRepository userRepository)
        {
            _nodeRepository = nodeRepository;
            _userRepository = userRepository;
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
            if (id != node.Id)
                return BadRequest();
            Node updatedNode = null;

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
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> PostNode([FromBody] Node node)
        {
            Node createdNode = null;
            var userId = int.Parse(ClaimsPrincipalHelper.GetClaimedUserIdentifier(this.User));
            var user = await _userRepository.GetByIdAsync(userId);

            node.Owner = user;

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