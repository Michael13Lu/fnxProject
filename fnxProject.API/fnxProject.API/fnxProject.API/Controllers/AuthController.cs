using fnxProject.API.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace fnxProject.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration configuration;

		public AuthController(IConfiguration configuration)
		{
			this.configuration = configuration;
		}

		//כניסה
		[HttpPost("login")]
		public IActionResult Login([FromBody] User user)
		{
			// דוגמה לבדיקה של נתונים (יש להחליף בבדיקה שלכם)
			if (IsValidUser(user))
			{
				var token = GenerateJwtToken(user.Name);
				return Ok(new { token });
			}

			return Unauthorized("שם משתמש או סיסמה שגויים.");
		}

		// יצירת טוקן
		private string GenerateJwtToken(string username)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, username),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.Name, username)
			};

			var token = new JwtSecurityToken(
				issuer: configuration["Jwt:Issuer"],
				audience: configuration["Jwt:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(60),
				signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private bool IsValidUser(User user)
		{
			return user.Name == "admin" && user.Password == "admin";
		}
	}
}
