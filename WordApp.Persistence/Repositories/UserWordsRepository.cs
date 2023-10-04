using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WordApp.Persistence.Interfaces;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.UserWordsDtos;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Persistence.Repositories
{
    public class UserWordsRepository : IUserWordsRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private int MinStage => _config.GetValue<int>("ExerciseOptions:MinStage");
        private int MaxStage => _config.GetValue<int>("ExerciseOptions:MaxStage");
        private int RepeatOnCoefficient => _config.GetValue<int>("ExerciseOptions:RepeatOnCoefficient");
        private int RepeatOnMaxStage => _config.GetValue<int>("ExerciseOptions:RepeatOnMaxStageDays");

        public UserWordsRepository(DataContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<List<UserWordDto>> AddUserWordsAsync(Guid userId, UserWordRequest[] words)
        {
            var user = await _context.Users
               .Where(u => u.Id == userId)
               .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            List<UserWord> userWords = new List<UserWord>();
            foreach (var wordRequest in words)
            {
                UserWord userWord = new UserWord
                {
                    OriginalWord = wordRequest.OriginalWord,
                    TargetWord = wordRequest.TargetWord,
                    Definition = wordRequest.Definition,
                    OriginalContext = wordRequest.OriginalContext,
                    TargetContext = wordRequest.TargetContext,
                    OriginalLanguage = wordRequest.OriginalLanguage,
                    TargetLanguage = wordRequest.TargetLanguage,
                    Stage = MinStage,
                    RepeatOn = DateTimeOffset.Now.AddDays(1),
                    Theme = wordRequest.Theme,
                    User = user
                };
                userWords.Add(userWord);
            }

            await _context.UserWords.AddRangeAsync(userWords);
            await _context.SaveChangesAsync();

            return userWords.Select(x => _mapper.Map<UserWordDto>(x)).ToList();
        }

        public async Task<bool> DeleteUserWordsAsync(Guid userId, Guid[] words)
        {
            var wordsToDelete = await _context.UserWords.
                Where(w => words.Contains(w.Id) && w.User.Id == userId).
                ToListAsync();

            _context.UserWords.RemoveRange(wordsToDelete);

            return await _context.SaveChangesAsync() == words.Length;
        }

        public async Task<List<UserWordDto>> UpdateUserWordsAsync(Guid userId, UserWordDto[] words)
        {

            foreach (var userWordDto in words)
            {
                var userWord = await _context.UserWords.Where(w => w.Id == userWordDto.Id && w.User.Id == userId).FirstOrDefaultAsync();
                if (userWord == null) continue;

                userWord.Definition = userWordDto.Definition;
                userWord.OriginalWord = userWordDto.OriginalWord;
                userWord.TargetWord = userWordDto.TargetWord;
                userWord.TargetContext = userWordDto.TargetContext;
                userWord.OriginalContext = userWordDto.OriginalContext;
                userWord.Theme = userWordDto.Theme;
                userWord.TargetLanguage = userWordDto.TargetLanguage;
                userWord.OriginalLanguage = userWordDto.OriginalLanguage;
                userWord.Stage = userWordDto.Stage;
            }

            await _context.SaveChangesAsync();

            return words.Select(x => _mapper.Map<UserWordDto>(x)).ToList();
        }

        public async Task<bool> ChangeUserWordStage(Guid userId, Guid userWordId, int stage)
        {
            var userWord = await _context.UserWords.
                Where(w => w.Id == userWordId && w.User.Id == userId).
                FirstOrDefaultAsync();

            if (userWord == null)
            {
                throw new ArgumentNullException(nameof(userWord));
            }

            userWord.Stage = stage;
            userWord.RepeatOn = stage == MaxStage ?
                DateTimeOffset.Now.AddDays(RepeatOnMaxStage)
                : DateTimeOffset.Now.AddDays((stage + 1) * RepeatOnCoefficient);

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<UserWordDto>> GetUserWordsAsync(Guid userId)
        {
            return await _context.UserWords
                .Where(w => w.User.Id == userId)
                .Select(u => _mapper.Map<UserWordDto>(u))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserWordDto>> GetUserWordsToRepeatAsync(Guid userId)
        {
            return await _context.UserWords.Where(x => x.User.Id==userId && x.RepeatOn <= DateTimeOffset.Now).Select(x=>_mapper.Map<UserWordDto>(x)).ToListAsync();
        }
    }
}
