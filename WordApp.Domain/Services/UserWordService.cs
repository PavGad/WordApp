using Microsoft.Extensions.Configuration;
using WordApp.Domain.Interfaces;
using WordApp.Persistence.Interfaces;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.UserWordsDtos;

namespace WordApp.Domain.Services
{
    public class UserWordService : IUserWordService
    {
        private readonly IUserWordsRepository _userWordRepo;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private int MinStage => _config.GetValue<int>("ExerciseOptions:MinStage");
        private int MaxStage => _config.GetValue<int>("ExerciseOptions:MaxStage");

        public UserWordService(IUserWordsRepository userWordsRepository, IJwtService jwtService, IUserRepository userRepository, IConfiguration config)
        {
            _userWordRepo = userWordsRepository;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _config = config;
        }

        public async Task<List<UserWordDto>> GetUserWordsAsync(Guid userId)
        {
            return await _userWordRepo.GetUserWordsAsync(userId);
        }

        public async Task<List<UserWordDto>> AddUserWordsAsync(Guid userId, UserWordRequest[] userWords)
        {
            if (userWords == null || userWords.Length == 0)
            {
                throw new ArgumentException(nameof(userWords));
            }

            return await _userWordRepo.AddUserWordsAsync(userId, userWords);
        }

        public async Task<bool> DeleteUserWordsAsync(Guid userId, Guid[] userWords)
        {
            if (userWords.Length == 0)
            {
                throw new ArgumentException(nameof(userWords));
            }

            return await _userWordRepo.DeleteUserWordsAsync(userId, userWords);
        }

        public async Task<List<UserWordDto>> UpdateUserWordsAsync(Guid userId, UserWordDto[] userWords)
        {
            if (userWords == null || userWords.Length == 0)
            {
                throw new ArgumentException(nameof(userWords));
            }

            return await _userWordRepo.UpdateUserWordsAsync(userId, userWords);
        }

        public async Task<bool> ChangeUserWordStageAsync(Guid userId, Guid userWord, int stage)
        {
            stage = stage < MinStage ? MinStage : stage;
            stage = stage > MaxStage ? MaxStage : stage;

            return await _userWordRepo.ChangeUserWordStage(userId, userWord, stage);
        }

        public async Task<UserWordsNotificationDto> GetUserWordsNotificationsAsync(Guid userid)
        {
            var words = await _userWordRepo.GetUserWordsToRepeatAsync(userid);
            return new UserWordsNotificationDto()
            {
                WordsToRepeat = words.Count()
            };
        }
    }
}
