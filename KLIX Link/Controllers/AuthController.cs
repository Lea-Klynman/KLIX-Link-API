using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.IServices;
using KLIX_Link_Core.Services;
using KLIX_Link_Service;
using KLIX_Link_Service.Post_Model;
using KLIX_Link_Service.Post_Modle;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KLIX_Link.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public AuthController(IConfiguration configuration, IUserService userService, IMapper mapper, IAuthService authService)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpPost("login")]

        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!EmailValidator.IsValidEmail(loginModel.Email))
            {
                return BadRequest("Email Not valid");
            }
            var res = await _userService.LoginAsync(loginModel.Email, loginModel.Password);
            if (res == null)
            {
                return NotFound();
            }
            if (res.IsActive == false)
                return Unauthorized();

            var tokenString = _authService.GenerateJwtToken(res.Name, res.Roles.Select(role => role.RoleName).ToArray());
            return Ok(new { Token = tokenString, user = res });

        }


        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterPostModel userRegister )
        {
            if (!EmailValidator.IsValidEmail(userRegister.User.Email))
            {
                return BadRequest("Email Not valid");

            }

                var res = await _userService.RegisterAsync(_mapper.Map<UserDto>(userRegister.User), userRegister.Roles);
            if (res == null)
            {
                return BadRequest();
            }

                var tokenString = _authService.GenerateJwtToken(res.Name, userRegister.Roles);
                return Ok(new { Token = tokenString, user = res });
            

        }


    }
}
