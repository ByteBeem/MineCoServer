using Microsoft.AspNetCore.Mvc;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Firebase.Database;
using Newtonsoft.Json;


[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SignupService signupService;

    public UsersController()
    {
        signupService = new SignupService();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest request)
    {
        try
        {
            
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and password are required.");

            
            if (await signupService.IsEmailExists(request.Email))
                return Conflict("Email already exists.");

            
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password, BCrypt.Net.BCrypt.GenerateSalt(12));

           
            User user = new User
            {
                Email = request.Email,
                HashedPassword = hashedPassword,
                FullName = request.FullName,
                Age = request.Age
            };

            
            await signupService.AddUser(user);

            
            var token = GenerateJwtToken(request.Email);

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
           
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                return BadRequest("Email and password are required.");

            
            var user = await signupService.GetUserByEmail(request.Email);
            
           
            if (user == null)
                return BadRequest("Invalid email or password.");

            
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.HashedPassword))
                return BadRequest("Invalid email or password.");

            
            var token = GenerateJwtToken(request.Email);

            return Ok(new { Token = token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    private string GenerateJwtToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("DonaldRSA04?????");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("email", email) }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

public class SignupRequest
{
    public string FullName { get; set; }
    public string Age { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class User
{
    public string FullName { get; set; }
    public string Age { get; set; }
    public string Email { get; set; }
    public string HashedPassword { get; set; }
}
public class SignupService
{
    private const string FirebaseDatabaseUrl = "https://mineco-16fc8-default-rtdb.asia-southeast1.firebasedatabase.app/";
    private readonly FirebaseClient firebaseClient;

    public SignupService()
    {
        firebaseClient = new FirebaseClient(FirebaseDatabaseUrl);
    }

    public async Task AddUser(User user)
    {
        await Task.Run(async () =>
        {
            await firebaseClient
                .Child("users")
                .PostAsync(JsonConvert.SerializeObject(user));
        });
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var snapshot = await firebaseClient
            .Child("users")
            .OnceAsync<User>();

        var userList = snapshot.Select(s => s.Object).ToList();
        var userWithEmail = userList.FirstOrDefault(user => user.Email == email);

        return userWithEmail;
    }

    public async Task<bool> IsEmailExists(string email)
    {
        var user = await GetUserByEmail(email);
        return user != null;
    }
}
