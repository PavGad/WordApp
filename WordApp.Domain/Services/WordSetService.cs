using AutoMapper;
using System.Text;
using WordApp.Domain.Interfaces;
using WordApp.Persistence.Interfaces;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Domain.Services
{
    public class WordSetService : IWordSetService
    {
        private readonly IWordSetRepository _wordSetRepository;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;

        public WordSetService(IWordSetRepository wordSetRepository, IJwtService jwtService, IUserRepository userRepository, IImageRepository imageRepository, IMapper mapper)
        {
            _wordSetRepository = wordSetRepository;
            _jwtService = jwtService;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
            _mapper = mapper;
        }
        public async Task<WordSetDto> AddWordSetAsync(Guid userId, WordSetRequest wordSetRequest)
        {
            if (wordSetRequest == null)
            {
                throw new ArgumentException("empty request");
            }
            if (await _wordSetRepository.GetWordSetByName(wordSetRequest.Name) != null)
            {
                throw new ArgumentException("set name must be unique");
            }

            var imageName = Convert.ToBase64String(Encoding.UTF8.GetBytes(wordSetRequest.Name + DateTime.UtcNow.ToString()));
            var imageUrl = await _imageRepository.AddImage(wordSetRequest.CoverImageBase64, imageName);

            return await _wordSetRepository.AddNewWordSet(userId, imageUrl, wordSetRequest);
        }

        public async Task<bool> CheckWordSetUniquenesAsync(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return true;
            }
            return await _wordSetRepository.GetWordSetByName(name) == null;
        }

        public async Task<ComplaintDto> ComplainAsync(Guid userId, ComplaintRequest compalinRequest)
        {
            if (compalinRequest == null)
            {
                throw new ArgumentException("empty request");
            }
            return await _wordSetRepository.AddComplaint(userId, compalinRequest);
        }

        public async Task<WordSetGetDto> GetWordSetsAsync(WordSetFilterCriteria filterCriteria)
        {
            if (filterCriteria == null)
            {
                throw new ArgumentException("empty filterCriteria");
            }

            var wordSets = await _wordSetRepository.GetWordSets(filterCriteria);

            return new WordSetGetDto
            {
                WordSets = wordSets.Item1,
                NumberOfPages = wordSets.Item2
            };
        }

        public async Task<IEnumerable<ProposedWordDto>> ProposeWordsAsync(Guid userId, ProposedWordsRequest proposedWordsRequest)
        {
            if (proposedWordsRequest == null)
            {
                throw new ArgumentException("empty proposedWordsRequest");
            }

            return await _wordSetRepository.AddProposedWord(userId, proposedWordsRequest);
        }



        public async Task<List<WordSetDto>> GetUnconfirmedWordSetsAsync()
        {
            return await _wordSetRepository.GetUnconfirmedWordSets();
        }
        public async Task<WordSetDto> ConfirmWordSetAsync(Guid userId, ConfirmSetRequest confirmSetRequest)
        {
            if (confirmSetRequest == null || String.IsNullOrEmpty(confirmSetRequest.Id))
            {
                throw new ArgumentException("empty ConfirmSetRequest");
            }
            Guid setId = Guid.Parse(confirmSetRequest.Id);

            return await _wordSetRepository.ConfirmWordSetAsync(userId, setId);
        }

        public async Task<WordSetDto> UpdateWordSetAsync(Guid userId, WordSetUpdateRequest wordSetupdateRequset)
        {
            if (wordSetupdateRequset == null || wordSetupdateRequset.WordSet == null)
            {
                throw new ArgumentException("empty wordSet");
            }
            return await _wordSetRepository.UpdateWordSetAsync(userId, wordSetupdateRequset);
        }

        public async Task<bool> DeleteWordSetAsync(Guid wordSetId)
        {
            return await _wordSetRepository.DeleteWordSetAsync(wordSetId);
        }

        public async Task<List<ProposedWordDto>> GetProposedWordsAsync()
        {
            return await _wordSetRepository.GetProposedWordsAsync();
        }

        public async Task<WordSetDto> ConfirmProposedWordAsync(Guid userId, Guid proposedWordId)
        {
            return await _wordSetRepository.ConfirmProposedWordAsync(userId, proposedWordId);
        }

        public async Task<List<WordSetDto>> ConfirmProposedWordsAsync(Guid userId, Guid[] proposedWordsId)
        {
            return await _wordSetRepository.ConfirmProposedWordsAsync(userId, proposedWordsId);
        }

        public async Task<bool> DeleteProposedWords(Guid proposedWordId)
        {
            return await _wordSetRepository.DeleteProposedWordAsync(proposedWordId);
        }

        public async Task<List<ComplaintDto>> GetComplaintsAsync()
        {
            return await _wordSetRepository.GetComplaintsAsync();
        }

        public async Task<bool> DeleteComplaintAsync(Guid cmplaintId)
        {
            return await _wordSetRepository.DeleteComplaintAsync(cmplaintId);
        }

    }
}
