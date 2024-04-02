using Microsoft.AspNetCore.Mvc;
using BCrypt.Net;

namespace MineCoServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EncryptController : ControllerBase
    {
        [HttpGet("EncryptedText")]
        public IActionResult GetEncryptedText()
        {
            string password = "kjwfweghe";
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Ok($"Hello from EncryptedText endpoint! {hashedPassword}");
        }

        [HttpGet("DecryptedText")]
        public IActionResult GetDecryptedText()
        {
            return Ok("Hello from DecryptedText endpoint!");
        }
    }
}
