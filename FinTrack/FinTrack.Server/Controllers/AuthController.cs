using FinTrack.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrack.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;

        public AuthController(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var existing = await _userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return BadRequest(new { message = "An account with this email already exists." });

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(new { message = string.Join(", ", result.Errors.Select(e => e.Description)) });

            await _userManager.AddClaimsAsync(user, new[]
            {
                new Claim("firstName", dto.FirstName),
                new Claim("lastName",  dto.LastName),
            });

            return Ok(BuildAuthResponse(user, dto.FirstName));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized(new { message = "Invalid email or password." });

            var claims = await _userManager.GetClaimsAsync(user);
            var firstName = claims.FirstOrDefault(c => c.Type == "firstName")?.Value ?? "";

            return Ok(BuildAuthResponse(user, firstName));
        }

        private AuthResponseDto BuildAuthResponse(IdentityUser user, string firstName)
        {
            var token = GenerateToken(user, firstName);
            return new AuthResponseDto
            {
                Token     = token,
                UserId    = user.Id,
                Email     = user.Email!,
                FirstName = firstName,
                Expiry    = DateTime.UtcNow.AddHours(24)
            };
        }

        private string GenerateToken(IdentityUser user, string firstName)
        {
            var key   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
                new Claim("firstName", firstName),
            };

            var token = new JwtSecurityToken(
                issuer:             _config["Jwt:Issuer"],
                audience:           _config["Jwt:Audience"],
                claims:             claims,
                expires:            DateTime.UtcNow.AddHours(24),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
