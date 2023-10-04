using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Persistence.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task<UserDto> GetUserByIdAsync(Guid id);
        Task<UserDto> CreateUserAsync(UserCredentials request);
        Task<RefreshTokenDto> CreateRefreshTokenAsync(UserDto user);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
        Task<bool> CheckUsernameUniquenessAsync(string username);
    }
}
