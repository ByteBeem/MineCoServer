using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace MineCoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        [HttpGet("LoginHistory")]
        public IActionResult GetLoginHistory()
        {
            string password = "kjwfweghe";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok($"Hello from LoginHistory endpoint! {hashedPassword}");
        }

        [HttpGet("Transactions")]
        public IActionResult GetTransactions()
        {
            return Ok("Hello from Transactions endpoint!");
        }
    }
}
