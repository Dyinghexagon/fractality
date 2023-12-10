using Fractality.Services.UserServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Fractality.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersServices _usersServices;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUsersServices userServices, 
            ILogger<AuthController> logger
        )
        {
            _usersServices = userServices;
            _logger = logger;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login() {
            var userId = Guid.NewGuid().ToString();
            var claims = new List<Claim>() 
            {
                new (ClaimTypes.Name, userId),
                new ("access_token", GetAccessToken(userId)),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();
            authProperties.SetParameter("IsPersistent", true);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                new ClaimsPrincipal(claimsIdentity), 
                authProperties);

            return Ok("login!");
        }


        private static string GetAccessToken(string userId)
        {
            const string issuer = "localhost";
            const string audience = "localhost";

            var identity = new ClaimsIdentity(new List<Claim>
            {
               new Claim("sub", userId)
            });

            var bytes = Encoding.UTF8.GetBytes(userId);
            var key = new SymmetricSecurityKey(bytes);
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var handler = new JwtSecurityTokenHandler();

            var token = handler.CreateJwtSecurityToken(
              issuer, audience, identity,
              now, now.Add(TimeSpan.FromHours(1)),
              now, signingCredentials
            );

            return handler.WriteToken(token);
        }

        [HttpGet("logout")]
        public Task<IActionResult> Logout()
        {
            throw new NotImplementedException();
        }

    }
}
