using System.IdentityModel.Tokens.Jwt;
using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Domain.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(UserDto user);
        //IEnumerable<Claim> GetClaimsFromToken(string token);
        //Guid GetUserIdFromAccessToken(string token);
        JwtSecurityToken DecodeToken(string token);
    }
}
