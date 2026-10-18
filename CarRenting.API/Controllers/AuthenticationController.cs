using CarRenting.Repositories.Repo;
using CarRenting.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using CarRenting.DTOs.Request;
using CarRenting.API.Models;
using CarRenting.BusinessObjects.Models;
using CarRenting.DTOs.Reponse;

namespace FlowerBouquetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomerRepo _repository = new CustomerRepo();
        private readonly AppSetting _appSettings;
        private readonly AdminAccount admin;

        public AuthenticationController(IOptionsMonitor<AppSetting> appSettings, IOptionsMonitor<AdminAccount> adminSetting)
        {
            _appSettings = appSettings.CurrentValue;
            admin = adminSetting.CurrentValue;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (model.Email.ToLower().Equals(admin.Email.ToLower()) && model.Password.Equals(admin.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email,admin.Email),
                    new Claim(ClaimTypes.Name,"Admin"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.Admin)
                };

                var token = GetToken(authClaims);

                var reponse = new LoginResponse()
                {
                    Id = "-1",
                    Name = "Admin",
                    Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                };
                return Ok(reponse);
            }

            var user = await _repository.LoginAsync(model.Email, model.Password);
            if (user != null)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, user.CustomerName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, UserRoles.Customer)
                };
                var token = GetToken(authClaims);
                var reponse = new LoginResponse()
                {
                    Id = user.CustomerId.ToString(),
                    Name = user.CustomerName,
                    Jwt = new JwtSecurityTokenHandler().WriteToken(token),

                };
                return Ok(reponse);
            }
            return Unauthorized("Email or password are wrong");
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody]Customer model)
        {
            var userExists = await _repository.GetCustomerByEmail(model.Email);

            if (model.Email.ToLower().Equals(admin.Email.ToLower()) || (userExists != null && userExists.CustomerStatus != 0))
                return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });
            var result = await _repository.AddAsync(model);
            return Ok(result);
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.SecretKey));

            var token = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Audience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
