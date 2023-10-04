using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WordApp.Domain.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Domain.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        public string Key => _config.GetValue<string>("JwtSettings:Key");
        public string Issuer => _config.GetValue<string>("JwtSettings:ValidIssuer");
        public string Audience => _config.GetValue<string>("JwtSettings:ValidAudience");
        public int AccessTokenExpirationTimeMinute => _config.GetValue<int>("JwtSettings:AccessTokenExpirationTimeMinute");
        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(UserDto user)
        {
            TimeSpan exp = TimeSpan.FromMinutes(AccessTokenExpirationTimeMinute);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));

            var claims = new List<Claim>() {
                    new Claim("username", user.Username),
                    new Claim(ClaimTypes.Role, user.Role.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

            var jwt = new JwtSecurityToken(
             issuer: Issuer,
             audience: Audience,
             claims: claims,
             expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AccessTokenExpirationTimeMinute)),
             signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public JwtSecurityToken DecodeToken(string token)
        {
            return new JwtSecurityTokenHandler().ReadJwtToken(token);
        }
    }
}
