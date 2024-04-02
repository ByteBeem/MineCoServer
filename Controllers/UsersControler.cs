using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace MineCoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult GetLogin()
        {
            string password = "kjwfweghe";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok($"Hello from login endpoint! {hashedPassword}");
        }

        [HttpGet("signup")]
        public IActionResult GetSignup()
        {
            return Ok("Hello from signup endpoint!");
        }
    }
}
