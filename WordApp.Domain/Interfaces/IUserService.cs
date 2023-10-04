using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Domain.Interfaces
{
    public interface IUserService
    {
        Task<bool> CheckUsernameUniquenessAsync(string username);
        Task<UserInfo> GetUserInfoById(Guid id);
    }
}
