using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.Domain.Interface.User;
using Server.Dto.Request;
using Server.Dto.Response;
using Server.Entity;

namespace Server.Infrastructure.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "JwtScheme")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    private readonly IConfiguration _configuration;

    private readonly IUserRepository _userRepository;

    public UserController(ILogger<UserController> logger, IUserRepository userRepository, IConfiguration configuration)
    {
        _logger = logger;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    [HttpPost("User/SignUp")]
    [AllowAnonymous]
    public async Task<UserDto> Create(User user)
    {
        return new UserDto(await _userRepository.CreateUser(user));
    }

    [HttpGet("User/Users")]
    public async Task<IEnumerable<UserDto?>> GetAll()
    {
        return (await _userRepository.GetUsers()).Select(u => new UserDto(u));
    }

    [HttpGet("User/{Id}")]
    public async Task<UserDto?> Get(Guid Id)
    {
        return new UserDto(await _userRepository.GetUser(Id));
    }

    [HttpPost("User/Verify")]
    [AllowAnonymous]
    public async Task<bool> Verify([FromBody] VerifyDto request)
    {
        return await _userRepository.VerifyUser(request.UserName, request.Password);
    }

    [HttpPost("User/Login")]
    [AllowAnonymous]
    public async Task<ActionResult> Login([FromBody] VerifyDto request)
    {
        try
        {
            var LoginUser = await _userRepository.VerifyUser(request.UserName, request.Password);

            if(!LoginUser)
            {
                return BadRequest("Invalid Username or Password");
            }

            var user = await _userRepository.GetUserByUserName(request.UserName);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            string userToken = tokenHandler.WriteToken(token);
            return Ok(new { Token = userToken, UserName = user.UserName });
        }
        catch(Exception e)
        {
            return BadRequest($"Login Failed Due to: {e.Message}");
        }
    }
}
