using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Services;
using KLIX_Link_Service.Post_Modle;
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
        public UserController(IMapper mapper ,IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        // GET: api/<UserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserByIdAsync(int id )
        {
            if (id < 0)
                return BadRequest();
            var res=await _userService.GetUserByIdAsync(id);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }


        [HttpGet("email")]
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email) 
        {
            var res=await _userService.GetUserByEmailAsync(email);
            if(res == null)
                return NotFound();
            
            return Ok(res);
        }

        
        // PUT api/<UserController>/5
        [HttpPut("/name/{id}")]
        public async Task<ActionResult<bool>> UpdateName(int id, [FromBody] string name)
        {
            var res= await _userService.UpdateNameAsync(id, name);
            if(!res)
                return NotFound();
            return Ok(res);
        }

        [HttpPut("/password/{id}")]
public async Task<ActionResult<bool>> UpdatePassword(int id, [FromBody] string password)
        {
            var res = await _userService.UpdatePasswordAsync(id, password);
            if(!res)
                return NotFound();
            return Ok(res);
        }
        // DELETE api/<UserController>/5
        [HttpDelete("/{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            var res=await _userService.DeleteUserAsync(id); 
            if(!res)
                return NotFound();
            return Ok(res);
        }
    }
}
