using Essays.WebApi.Auth;
using Essays.WebApi.Data.Interfaces;
using Essays.WebApi.DTOs;
using Essays.WebApi.Models;
using Essays.WebApi.Repositories.Interfaces;
using Essays.WebApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Essays.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthorRepository _authorRepository;
        private readonly IRandomGenerator _randomGenerator;
        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration configuration,
            IAuthorRepository authorRepository,
            IRandomGenerator randomGenerator,
            IUserService userService)
        {
            _configuration = configuration;
            _authorRepository = authorRepository;
            _randomGenerator = randomGenerator;
            _userService = userService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Register([FromBody] RegisterFormDto registerForm)
        {
            if (registerForm == null)
            {
                return BadRequest("Register form is null!");
            }

            var existingAuthor = await _authorRepository.GetAuthorByLogin(registerForm.Login);
            if (existingAuthor == null)
            {
                return BadRequest("Such author already exists");
            }

            var author = new Author()
            {
                AuthorId = _randomGenerator.GetRandomId(),
                FirstName = registerForm.FirstName.Trim(),
                LastName = registerForm.LastName.Trim(),
                Login = registerForm.Login.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerForm.Password)
            };

            var created = await _authorRepository.CreateAuthor(author);
            if (!created)
            {
                return StatusCode(500, "Failed to create a new country");
            }

            return Ok(author.AuthorId);
        }

        [HttpPost("Login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginFormDto loginForm)
        {
            if (loginForm == null)
            {
                return BadRequest("Login form is null!");
            }

            var author = await _authorRepository.GetAuthorByLogin(loginForm.Login);
            if (author == null)
            {
                return BadRequest("Author not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(loginForm.Password, author.PasswordHash))
            {
                return BadRequest("Wrong password");
            }

            var token = CreateToken(author);

            var refreshToken = GenerateRefreshToken();
            var isRefreshTokenSet = await SetRefreshToken(refreshToken, author);
            if (!isRefreshTokenSet)
            {
                return BadRequest("Can't generate refresh token");
            }

            return Ok(token);
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        private string CreateToken(Author user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Login", user.Login),
                new Claim("AuthorId", user.AuthorId),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = _randomGenerator.GetRandomString(64),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async Task<bool> SetRefreshToken(RefreshToken newRefreshToken, Author user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;

            return await _authorRepository.UpdateAuthor(user);
        }
    }
}
