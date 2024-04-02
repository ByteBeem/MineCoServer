using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace MineCoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        [HttpGet("Deposit")]
        public IActionResult GetDeposit()
        {
            string password = "kjwfweghe";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok($"Hello from Deposit endpoint! {hashedPassword}");
        }

        [HttpGet("Withdraw")]
        public IActionResult GetWithdraw()
        {
            return Ok("Hello from Withdraw endpoint!");
        }
    }
}
