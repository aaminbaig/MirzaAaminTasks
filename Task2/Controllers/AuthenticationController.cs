using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Task2.Models;

namespace Task2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly TaskDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _applicationSettings;

        public AuthenticationController(TaskDbContext dbContext, IConfiguration configuration, IOptions<AppSettings> applicationSettings)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _applicationSettings = applicationSettings.Value; 
        }

        [HttpPost("Register")]
        public async Task<IActionResult> EnterUserData(register model)
        {

            var newuser = new User { username = model.username, Role = model.Role};

            if (model.confirmpassword == model.password)
            {
                using (HMACSHA512? hmac = new HMACSHA512())
                {
                    newuser.passwordsalt = hmac.Key;
                    newuser.passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.password));
                }
            }
            else
            {
                return BadRequest("Passwords do not match");
            }

            _dbContext.Users.Add(newuser);
            await _dbContext.SaveChangesAsync();
            return Ok(newuser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(login userRequest)
        {

            var users = await _dbContext.Users.ToListAsync();

            var user = users.Where(x => x.username == userRequest.username).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Username or password wa invalid");
            }

            var match = CheckPassword(userRequest.password, user);

            if (!match)
            {
                return BadRequest("Username or password wa invalid");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_applicationSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.username), new Claim(ClaimTypes.Role, user.Role), new Claim("id", user.userid.ToString())}),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token);

            return Ok(new {token = encrypterToken, username =  user.username});
        }

        private bool CheckPassword(string password, User user)
        {
            bool result;

            using (HMACSHA512?hmac = new HMACSHA512(user.passwordsalt))
            {
                var compute = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                result = compute.SequenceEqual(user.passwordhash);
            }

            return result;
        }
    }
}
