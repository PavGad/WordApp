using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<LogInResponse> LogInAsync(UserCredentials loginRequest);
        Task<SignUpResponse> SignUpAsync(UserCredentials loginRequest);
        Task<bool> LogOutAsync(string refreshToken);
        Task<LogInResponse> RefreshTokenAsync(string accessToken, string refreshToken);
        Task<bool> CheckUsernameUniquenessAsync(string username);
    }
}
