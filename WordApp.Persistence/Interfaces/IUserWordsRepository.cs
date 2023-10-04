using WordApp.Shared.Dtos.UserWordsDtos;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Persistence.Interfaces
{
    public interface IUserWordsRepository
    {
        Task<List<UserWordDto>> AddUserWordsAsync(Guid userId, UserWordRequest[] words);
        Task<bool> DeleteUserWordsAsync(Guid userId, Guid[] words);
        Task<List<UserWordDto>> UpdateUserWordsAsync(Guid userId, UserWordDto[] words);
        Task<bool> ChangeUserWordStage(Guid userId, Guid userWordId, int stage);
        Task<List<UserWordDto>> GetUserWordsAsync(Guid userId);
        Task<IEnumerable<UserWordDto>> GetUserWordsToRepeatAsync(Guid userId);
    }
}
