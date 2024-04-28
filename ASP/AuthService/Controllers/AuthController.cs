using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using AuthService.Data;
using MongoDB.Driver;
using BCrypt.Net;
namespace BCrypt.Net;

[Route("user/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    //private readonly IMongoCollection<User> _users;
    private readonly IConfiguration _configuration;
    private readonly MongoDbContext _dbContext;

    public AuthController(MongoDbContext dbContext, IConfiguration configuration)
    {
       
        _dbContext = dbContext;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLogin)
    {
        var user = await _dbContext.Users.Find(u => u.Username == userLogin.Username).FirstOrDefaultAsync();
        if (user == null || !BCrypt.Verify(userLogin.Password, user.Password))
            return Unauthorized("Invalid credentials.");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiresInMinutes"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}
