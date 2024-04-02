using Microsoft.AspNetCore.Mvc;

namespace MineCoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("login")]
        public IActionResult GetLogin()
        {
            return Ok("Hello from login endpoint!");
        }

        [HttpGet("signup")]
        public IActionResult GetSignup()
        {
            return Ok("Hello from login signup!");
        }
    }
}
