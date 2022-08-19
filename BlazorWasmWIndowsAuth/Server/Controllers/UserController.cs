using BlazorWasmWIndowsAuth.Shared;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorWasmWIndowsAuth.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserController(IHttpContextAccessor httpContextAccessor , IConfiguration configuration)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._configuration = configuration;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
        [Route("GetUserName")]
        public IActionResult GetUserName()
        {
            string? login = _httpContextAccessor!.HttpContext!.User?.Identity?.Name?.Replace("tp\\", "").Replace("TP\\", "");
            return Ok(login!);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
        [Route("GetUser")]
        public  UserModel GetUser()
        {
            UserModel um = new UserModel();

            um.Login = _httpContextAccessor!.HttpContext!.User?.Identity?.Name?.Split("\\").Last();

            um.Roles.Add("admin");// in real life take this from DB etc.
            um.JWT = CreateJWT(um.Login);
            return um;
        }


        private string? CreateJWT(string? userName)
        {
            if (userName == null) return null;
            var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetRequiredSection("JWT:Key").Value));
            var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Sub, userName)
               /// new Claim(JwtRegisteredClaimNames.Email, user.Email),    // new Claim(JwtRegisteredClaimNames.Jti, user.Email) // NOTE: this could a unique ID assigned to the user by a database
			};

            var token = new JwtSecurityToken(issuer: _configuration.GetRequiredSection("JWT:Issuer").Value, audience: _configuration.GetRequiredSection("JWT:Issuer").Value, claims: claims, expires: DateTime.Now.AddMinutes(int.Parse(_configuration.GetRequiredSection("JWT:TimeValid").Value)), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
