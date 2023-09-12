using H3Shoes.Models;
using H3Shoes.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace H3Shoes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthManagerController : ControllerBase
    {
        private readonly ILogger<AuthManagerController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        public AuthManagerController(
            ILogger<AuthManagerController> logger,
            UserManager<IdentityUser> userManager,
            IOptionsMonitor<JwtConfig> optionsMonitor)
        {
            _logger = logger;
            _userManager = userManager;
            _jwtConfig = optionsMonitor.CurrentValue;
        }

        [HttpPost]
        [Route("Register")]
        // [EnableCors("AllowOrigin")]
        // [EnableCors("FrontEnd")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (ModelState.IsValid)
            {
                // check if email exist
                var emailExist = await _userManager.FindByEmailAsync(request.Email);

                if (emailExist is not null)
                {
                    return BadRequest(error: "Email already exist");
                }

                var newUser = new IdentityUser()
                {
                    Email = request.Email,
                    UserName = request.Name
                };

                var isCreated = await _userManager.CreateAsync(newUser, request.Password);

                if (isCreated.Succeeded)
                {
                    // generate token

                    return Ok(new RegistrationRequestRespone()
                    {
                        Result = true,
                        Token = GenerateJwtToken(newUser)
                    });

                }

                return BadRequest(error: isCreated.Errors.Select(x => x.Description).ToList());
            }
            return BadRequest(error: "Invalid request payload");
        }

        [HttpPost]
        [Route("Login")]
        // [EnableCors("AllowOrigin")]
        // [EnableCors("FrontEnd")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(requestDto.Email);

                if (existingUser is null)
                    return BadRequest("Invalid user");

                var isPasswordValid = await _userManager.CheckPasswordAsync(existingUser, requestDto.Password);

                if (isPasswordValid)
                {
                    var token = GenerateJwtToken(existingUser);

                    return Ok(new LoginRequestResponse()
                    {
                        Token = token,
                        Result = true

                    });
                }
                return BadRequest("Wrong password");
            }
            return BadRequest("Invalid authentication");
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new[]{
                    new Claim("Id", value:user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, value:user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, value:user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, value:Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    algorithm: SecurityAlgorithms.HmacSha512)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }

    }
}
