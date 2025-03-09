using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using KLIX_Link_Core.DTOS;
using KLIX_Link_Core.Entities;
using KLIX_Link_Core.Services;
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

        public AuthController(IConfiguration configuration, IUserService userService,IMapper mapper)
        {
            _configuration = configuration;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("login")]

        public async Task<ActionResult> Login([FromBody] LoginModel loginModel)
        {
            var res = await _userService.LoginAsync(loginModel.Email, loginModel.Password);

            if (res != null)
            {

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, res.Name),
                new Claim(ClaimTypes.Role,"User")
            };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("JWT:Issuer"),
                    audience: _configuration.GetValue<string>("JWT:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, user = res });
            }
            return Unauthorized();

        }


        [HttpPost("loginAdmin")]

        public async Task<ActionResult> LoginAdmin([FromBody] LoginModel loginModel)
        {
            var res = await _userService.LoginAsync(loginModel.Email, loginModel.Password);

            if (res != null)
            {

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, res.Name),
                new Claim(ClaimTypes.Role,"Admin")
            };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("JWT:Issuer"),
                    audience: _configuration.GetValue<string>("JWT:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, user = res });
            }
            return Unauthorized();

        }

        // POST api/<UserController>
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] UserPostmodel user)
        {
            var res = await _userService.RegisterAsync(_mapper.Map<UserDto>(user));
            if (res != null)
            {

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, res.Name),
                new Claim(ClaimTypes.Role,"User")
            };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("JWT:Issuer"),
                    audience: _configuration.GetValue<string>("JWT:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                Role r = Role.USER;
                bool  isUpdateRole= await  _userService.UpdateRoleAsync(res.Id, r);
                if (!isUpdateRole)
                {
                    return Ok(false);
                }
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, user = res });
            }
            return Unauthorized();

        }


        [HttpPost("registerAdmin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] UserPostmodel user)
        {
            var res = await _userService.RegisterAsync(_mapper.Map<UserDto>(user));
            if (res != null)
            {

                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, res.Name),
                new Claim(ClaimTypes.Role,"Admin")
            };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWT:Key")));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: _configuration.GetValue<string>("JWT:Issuer"),
                    audience: _configuration.GetValue<string>("JWT:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(6),
                    signingCredentials: signinCredentials
                );
                Role r = Role.ADMIN;
                bool isUpdateRole = await  _userService.UpdateRoleAsync(res.Id, r);
                if (!isUpdateRole) {
                    return Ok(false);
                }
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return Ok(new { Token = tokenString, user = res });
            }
            return Unauthorized();

        }
    }
}
