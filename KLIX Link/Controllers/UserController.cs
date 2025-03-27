using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Services;
using KLIX_Link_Service.Post_Modle;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KLIX_Link.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        readonly IMapper _mapper;
        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }


        // GET: api/<UserController>
        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id)
        {
            if (id < 0)
                return BadRequest();
            var res = await _userService.GetUserByIdAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }


        [HttpGet("email")]
        [Authorize(Policy = "UserOrAdmin")]
        public async Task<ActionResult<UserDto>> GetUserByEmailAsync(string email)
        {
            var res = await _userService.GetUserByEmailAsync(email);
            if (res == null)
                return NotFound();

            return Ok(res);
        }


        // PUT api/<UserController>/5
        [HttpPut("name/{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<bool>> UpdateNameAsync(int id, [FromBody] string name)
        {
            var res = await _userService.UpdateNameAsync(id, name);
            if (!res)
                return NotFound();
            return Ok(res);
        }

        [HttpPut("password/{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<bool>> UpdatePasswordAsync(int id, [FromBody] string password)
        {
            var res = await _userService.UpdatePasswordAsync(id, password);
            if (!res)
                return NotFound();
            return Ok(res);
        }


        [HttpPut("enable/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<bool>> EnableUserAsync(int id)
        {
            var res = await _userService.EnableUserAsync(id);
            if (!res)
                return NotFound();
            return Ok(res);
        }

        [HttpPut("disable/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<bool>> DisableUserAsync(int id)
        {
            var res = await _userService.DisableUserAsync(id);
            if (!res)
                return NotFound();
            return Ok(res);
        }
        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<ActionResult<bool>> DeleteUserAsync(int id)
        {
            var res = await _userService.DeleteUserAsync(id);
            if (!res)
                return NotFound();
            return Ok(res);
        }
    }
}
