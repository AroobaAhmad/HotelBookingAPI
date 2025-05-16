using HotelBookingAPI.JwtToken;
using HotelBookingAPI.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using static HotelBookingAPI.Models.Login;

namespace HotelBookingAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = JwtTokenGenerator.GenerateToken("admin-id", _config["JwtSettings:Secret"]);
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
    }
}
