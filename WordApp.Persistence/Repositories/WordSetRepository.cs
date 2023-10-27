using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WordApp.Persistence.Extentions;
using WordApp.Persistence.Interfaces;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Persistence.Repositories
{
    public class WordSetRepository : IWordSetRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public int PageSize => _config.GetValue<int>("PageSize");

        public WordSetRepository(DataContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<ComplaintDto> AddComplaint(Guid userId, ComplaintRequest newComplaintDto)
        {
            var reason = await _context.ComplaintReasons
                .Where(x => x.Id == newComplaintDto.Reason)
                .FirstOrDefaultAsync();

            var set = await _context.WordSets
                .Where(x => x.Id == newComplaintDto.WordSetId).FirstOrDefaultAsync();
            if (set == null)
            {
                throw new ArgumentException("WordSets doesn`t exist");
            }

            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            var complaint = new Complaint()
            {
                CreatedOn = DateTimeOffset.Now,
                Message = newComplaintDto.Message,
                Reason = reason,
                User = user,
                WordSet = set
            };

            _context.Complaints.Add(complaint);
            _context.SaveChanges();
            return _mapper.Map<ComplaintDto>(complaint);
        }

        public async Task<WordSetDto> AddNewWordSet(Guid userId, string imageUrl, WordSetRequest wordSetRequest)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            var wordSet = new WordSet()
            {
                CoverImageUrl = imageUrl,
                CreatedBy = user,
                CreatedOn = DateTimeOffset.Now,
                Description = wordSetRequest.Description,
                Name = wordSetRequest.Name,
                UpdatedOn = DateTimeOffset.Now,
                Level = wordSetRequest.Level,
                OriginalLanguage = wordSetRequest.OriginalLanguage,
                TargetLanguage = wordSetRequest.TargetLanguage,
                Confirmed = false,
                Words = new List<Word>()
            };

            var originalWords = wordSetRequest.Words.Select(x => x.OriginalWord).ToList();

            var existingWords = await _context.Words.
                Where(x => x.OriginalLanguage == wordSetRequest.OriginalLanguage && x.TargetLanguage == wordSetRequest.TargetLanguage && originalWords.Contains(x.OriginalWord))
                .GroupBy(x => x.OriginalWord)
                .ToDictionaryAsync(x => x.Key, x => x.ToList());

            foreach (var wordRequest in wordSetRequest.Words)
            {
                var existinWordsWithSameOriginal = existingWords.ContainsKey(wordRequest.OriginalWord) ? existingWords[wordRequest.OriginalWord] : null;

                var word = existinWordsWithSameOriginal?.Where(a =>
                    a.TargetWord == wordRequest.TargetWord
                    && a.OriginalContext == wordRequest.OriginalContext
                    && a.TargetContext == wordRequest.TargetContext
                    && a.Definition == wordRequest.Definition).FirstOrDefault();

                if (word != null)
                {
                    wordSet.Words.Add(word);
                }
                else
                {
                    var newWord = new Word()
                    {
                        Definition = wordRequest.Definition,
                        OriginalWord = wordRequest.OriginalWord,
                        TargetWord = wordRequest.TargetWord,
                        OriginalContext = wordRequest.OriginalContext,
                        TargetContext = wordRequest.TargetContext,
                        OriginalLanguage = wordSetRequest.OriginalLanguage,
                        TargetLanguage = wordSetRequest.TargetLanguage,
                        User = user,
                        WordSets = new List<WordSet> { wordSet }
                    };
                    wordSet.Words.Add(newWord);
                }
            }
            await _context.WordSets.AddAsync(wordSet);
            await _context.SaveChangesAsync();

            return _mapper.Map<WordSetDto>(wordSet);
        }

        public async Task<IEnumerable<ProposedWordDto>> AddProposedWord(Guid userId, ProposedWordsRequest proposedWordsRequest)
        {
            var user = await _context.Users
                .Where(x => x.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            var wordSet = await _context.WordSets
                .Where(x => x.Id == proposedWordsRequest.WordSetId).FirstOrDefaultAsync();
            if (wordSet == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            var proposedWordDtos = new List<ProposedWordDto>();

            foreach (var item in proposedWordsRequest.Words)
            {
                var proposedWord = new ProposedWord()
                {
                    Definition = item.Definition,
                    OriginalContext = item.OriginalContext,
                    TargetContext = item.TargetContext,
                    OriginalWord = item.OriginalWord,
                    TargetWord = item.TargetWord,
                    User = user,
                    WordSet = wordSet,
                };
                await _context.ProposedWords.AddAsync(proposedWord);
                proposedWordDtos.Add(_mapper.Map<ProposedWordDto>(proposedWord));
            }

            await _context.SaveChangesAsync();
            return proposedWordDtos;
        }

        public async Task<WordSet> GetWordSetByName(string name)
        {
            return await _context.WordSets
                .Where(x => x.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }

        public async Task<(List<WordSetDto>, int)> GetWordSets(WordSetFilterCriteria filterCriteria)
        {
            var wordSets = await _context.WordSets
                .Where(s => s.Confirmed)
                .Include(s => s.Words)
                .WhereIf(!string.IsNullOrWhiteSpace(filterCriteria.SearchText),
                   s => s.Description.ToLower().Contains(filterCriteria.SearchText.ToLower())
                         || s.Name.ToLower().Contains(filterCriteria.SearchText.ToLower()))
                .WhereIf(filterCriteria.Levels != null && filterCriteria.Levels?.Count != 0, s => filterCriteria.Levels.Contains(s.Level))
                .OrderByDescending(c => c.CreatedOn)
                .Skip(filterCriteria.PageNumber * PageSize)
                .Take(PageSize)
                .ToListAsync();

            var pages = await _context.WordSets.Where(s => s.Confirmed).CountAsync();

            return (wordSets.Select(x => _mapper.Map<WordSetDto>(x)).ToList(), (pages + PageSize - 1) / PageSize);
        }

        public async Task<List<WordSetDto>> GetUnconfirmedWordSets()
        {
            return await _context.WordSets
                .Include(x => x.Words)
                .Where(x => x.Confirmed == false)
                .Select(x => _mapper.Map<WordSetDto>(x))
                .ToListAsync();
        }

        public async Task<WordSetDto> ConfirmWordSetAsync(Guid userId, Guid wordSetId)
        {
            var wordSet = await _context.WordSets.Include(x => x.Words).Where(x => x.Id == wordSetId).FirstOrDefaultAsync();
            if (wordSet == null)
            {
                throw new ArgumentException("wordSet doesn`t exist");
            }

            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            wordSet.Confirmed = true;
            wordSet.ConfirmedBy = user;
            wordSet.ConfirmedOn = DateTimeOffset.Now;
            wordSet.UpdatedOn = DateTimeOffset.Now;

            await _context.SaveChangesAsync();

            return _mapper.Map<WordSetDto>(wordSet);
        }

        public async Task<WordSetDto> UpdateWordSetAsync(Guid userId, WordSetUpdateRequest wordSetUpdateRequset)
        {
            var wordSet = await _context.WordSets
                .Include(x => x.Words)
                .Where(x => x.Id == wordSetUpdateRequset.WordSet.Id)
                .FirstOrDefaultAsync();
            if (wordSet == null)
            {
                throw new ArgumentException("wordSet doesn`t exist");
            }

            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            bool isAnyChange = false;

            isAnyChange = (wordSet.Name != wordSetUpdateRequset.WordSet.Name
                            || wordSet.Level != wordSetUpdateRequset.WordSet.Level
                            || wordSet.OriginalLanguage != wordSetUpdateRequset.WordSet.OriginalLanguage
                            || wordSet.TargetLanguage != wordSetUpdateRequset.WordSet.TargetLanguage
                ) ? true : false;

            if (isAnyChange)
            {
                wordSet.Name = wordSetUpdateRequset.WordSet.Name;
                wordSet.Level = wordSetUpdateRequset.WordSet.Level;
                wordSet.OriginalLanguage = wordSetUpdateRequset.WordSet.OriginalLanguage;
                wordSet.TargetLanguage = wordSetUpdateRequset.WordSet.TargetLanguage;
            }

            List<Word> wordsToRemove = new List<Word>();
            foreach (var wordFromset in wordSet.Words)
            {
                if (!wordSetUpdateRequset.WordSet.Words.Any(x => x.Id == wordFromset.Id))
                {
                    isAnyChange = true;
                    wordsToRemove.Add(wordFromset);
                }
            }

            foreach (var item in wordsToRemove)
            {
                wordSet.Words.Remove(item);
            }

            if (wordSetUpdateRequset.NewWords.Count > 0)
            {
                isAnyChange = true;
                //beware of shit code
                var originalWords = wordSetUpdateRequset.NewWords.Select(x => x.OriginalWord).ToList();

                var existingWords = await _context.Words.
                    Where(x => x.OriginalLanguage == wordSetUpdateRequset.WordSet.OriginalLanguage
                        && x.TargetLanguage == wordSetUpdateRequset.WordSet.TargetLanguage
                        && originalWords.Contains(x.OriginalWord))
                    .GroupBy(x => x.OriginalWord)
                    .ToDictionaryAsync(x => x.Key, x => x.ToList());

                foreach (var wordRequest in wordSetUpdateRequset.NewWords)
                {
                    var existinWordsWithSameOriginal = existingWords.ContainsKey(wordRequest.OriginalWord) ? existingWords[wordRequest.OriginalWord] : null;

                    var word = existinWordsWithSameOriginal?.Where(a =>
                        a.TargetWord == wordRequest.TargetWord
                        && a.OriginalContext == wordRequest.OriginalContext
                        && a.TargetContext == wordRequest.TargetContext
                        && a.Definition == wordRequest.Definition).FirstOrDefault();

                    if (word != null)
                    {
                        wordSet.Words.Add(word);
                    }
                    else
                    {
                        var newWord = new Word()
                        {
                            Definition = wordRequest.Definition,
                            OriginalWord = wordRequest.OriginalWord,
                            TargetWord = wordRequest.TargetWord,
                            OriginalContext = wordRequest.OriginalContext,
                            TargetContext = wordRequest.TargetContext,
                            OriginalLanguage = wordSet.OriginalLanguage,
                            TargetLanguage = wordSet.TargetLanguage,
                            User = user,
                            WordSets = new List<WordSet> { wordSet }
                        };
                        wordSet.Words.Add(newWord);
                    }
                }
                //
            }


            if (isAnyChange)
            {
                wordSet.UpdatedOn = DateTimeOffset.Now;
                await _context.SaveChangesAsync();
            }

            return _mapper.Map<WordSetDto>(wordSet);
        }

        public async Task<bool> DeleteWordSetAsync(Guid wordSetId)
        {
            var wordSet = await _context.WordSets.Where(w => w.Id == wordSetId).FirstOrDefaultAsync();
            if (wordSet == null)
            {
                throw new ArgumentException("wordSet doesn`t exist");
            }

            _context.WordSets.Remove(wordSet);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ProposedWordDto>> GetProposedWordsAsync()
        {
            return await _context.ProposedWords
                .Include(x => x.User)
                .Include(x => x.WordSet)
                .Select(s => _mapper.Map<ProposedWordDto>(s))
                .ToListAsync();
        }

        public async Task<WordSetDto> ConfirmProposedWordAsync(Guid userId, Guid proposedWordId)
        {
            var proposedWord = await _context.ProposedWords
                .Include(x => x.WordSet).ThenInclude(x => x.Words)
                .Where(p => p.Id == proposedWordId).FirstOrDefaultAsync();
            if (proposedWord == null)
            {
                throw new ArgumentException("porposedWord doesn`t exist");
            }

            var user = await _context.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }

            var existingWord = await _context.Words
                .Where(x => x.TargetWord == proposedWord.TargetWord
                        && x.OriginalWord == proposedWord.OriginalWord
                        && x.TargetContext == proposedWord.TargetContext
                        && x.OriginalContext == proposedWord.OriginalContext
                        && x.Definition == proposedWord.Definition
                        )
                .FirstOrDefaultAsync();

            if (existingWord == null)
            {
                var word = new Word()
                {
                    Definition = proposedWord.Definition,
                    OriginalWord = proposedWord.OriginalWord,
                    TargetWord = proposedWord.TargetWord,
                    OriginalContext = proposedWord.OriginalContext,
                    TargetContext = proposedWord.TargetContext,
                    OriginalLanguage = proposedWord.WordSet.OriginalLanguage,
                    TargetLanguage = proposedWord.WordSet.TargetLanguage,
                    User = user,
                    WordSets = new List<WordSet> { proposedWord.WordSet }
                };
                await _context.Words.AddAsync(word);
            }
            else
            {
                proposedWord.WordSet.Words.Add(existingWord);
                proposedWord.WordSet.UpdatedOn = DateTimeOffset.Now;
            }
            _context.ProposedWords.Remove(proposedWord);
            await _context.SaveChangesAsync();

            return _mapper.Map<WordSetDto>(proposedWord.WordSet);
        }

        public Task<List<WordSetDto>> ConfirmProposedWordsAsync(Guid usedId, Guid[] proposedWordId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteProposedWordAsync(Guid proposedWordId)
        {
            var proposedWord = await _context.ProposedWords
                .Where(s => s.Id == proposedWordId)
                .FirstOrDefaultAsync();

            if (proposedWord == null)
            {
                throw new ArgumentException("porposedWord doesn`t exist");
            }

            _context.ProposedWords.Remove(proposedWord);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ComplaintDto>> GetComplaintsAsync()
        {
            return await _context.Complaints
                .Include(x => x.User)
                .Include(x => x.Reason)
                .Include(x => x.WordSet).ThenInclude(x => x.Words)
                .Select(x => _mapper.Map<ComplaintDto>(x))
                .ToListAsync();
        }

        public async Task<bool> DeleteComplaintAsync(Guid complaintId)
        {
            var complaint = await _context.Complaints
                .Where(x => x.Id == complaintId)
                .FirstOrDefaultAsync();

            if (complaint == null)
            {
                throw new ArgumentException("complaint doesn`t exist");
            }

            _context.Complaints.Remove(complaint);

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
