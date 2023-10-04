using WordApp.Shared.Dtos.UserWordsDtos;

namespace WordApp.Domain.Interfaces
{
    public interface IUserWordService
    {
        Task<List<UserWordDto>> GetUserWordsAsync(Guid userId);
        Task<List<UserWordDto>> AddUserWordsAsync(Guid userId, UserWordRequest[] userWords);
        Task<bool> DeleteUserWordsAsync(Guid userId, Guid[] userWords);
        Task<List<UserWordDto>> UpdateUserWordsAsync(Guid userId, UserWordDto[] userWords);
        Task<bool> ChangeUserWordStageAsync(Guid userId, Guid userWord, int stage);
        Task<UserWordsNotificationDto> GetUserWordsNotificationsAsync(Guid userid);
    }
}
