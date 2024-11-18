using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookInfo.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequest authenticationRequest)
        {
            var user = ValidateCredentials(authenticationRequest.Username, authenticationRequest.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"]));
            var signing = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
        {
            new Claim("sub", user.UserId.ToString()),
            new Claim("Given Name", user.FirstName),
            new Claim("Parent Name", user.LastName),
            new Claim("book", user.Book)
        };

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: signing);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Ok(token);

        }

        private BookInfoUser ValidateCredentials(string? username, string? password)
        {
            return new BookInfoUser(1, username ?? "", "Paula", "Dockx", "Antwerp");
        }

        public class AuthenticationRequest
        {
            public string? Username { get; set; }
            public string? Password { get; set; }
        }

        private class BookInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Book { get; set; }

            public BookInfoUser(int userId, string userName, string firstName, string lastName, string book)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                Book = book;
            }
        }
    }

}
