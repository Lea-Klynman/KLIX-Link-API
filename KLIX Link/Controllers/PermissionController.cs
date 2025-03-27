using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KLIX_Link.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        readonly IPermissionService _permissionService;

        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }


        // GET: api/<PermissionController>
        [HttpGet]
        public async Task<ActionResult> GetAllPermissinAsync()
        {
            return Ok(await _permissionService.GetPermissionsAsync());
        }

        // GET api/<PermissionController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(int id)
        {
            var res = await _permissionService.GetPermissionByIdAsync(id);
            if (res == null) return NotFound();
            return Ok(res);
        }


        [HttpGet("/name/{name}")]
        public async Task<ActionResult> GetbyNameAsync(string name)
        {
            var res = await _permissionService.GetPermissionByNameAsync(name);
            if (res == null) return NotFound();
            return Ok(res);
        }


        // POST api/<PermissionController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] PermissionDTO permission)
        {
            var res = await _permissionService.AddPermissionAsync( permission);
            if (res == null) return BadRequest();
            return Ok(res);
        }


        // PUT api/<PermissionController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] PermissionDTO permission)
        {
            var res = await _permissionService.UpdatePermissionAsync(id, permission);
            if (res == null) return BadRequest();
            return Ok(res);
        }


        // DELETE api/<PermissionController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var res = await _permissionService.RemovePermissionAsync(id);
            if (!res) return NotFound();
            return Ok();
        }
    }
}
