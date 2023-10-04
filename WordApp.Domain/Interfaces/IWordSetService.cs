using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Domain.Interfaces
{
    public interface IWordSetService
    {
        Task<WordSetGetDto> GetWordSetsAsync(WordSetFilterCriteria filterCriteria);
        Task<WordSetDto> AddWordSetAsync(Guid userId, WordSetRequest wordSetRequest);
        Task<bool> CheckWordSetUniquenesAsync(string name);
        Task<IEnumerable<ProposedWordDto>> ProposeWordsAsync(Guid userId, ProposedWordsRequest proposedWordsRequest);
        Task<ComplaintDto> ComplainAsync(Guid userId, ComplaintRequest compalinRequest);
        Task<List<ComplaintDto>> GetComplaintsAsync();
        Task<List<ProposedWordDto>> GetProposedWordsAsync();
        Task<List<WordSetDto>> GetUnconfirmedWordSetsAsync();
        Task<WordSetDto> ConfirmWordSetAsync(Guid userId, ConfirmSetRequest confirmSetRequest);
        Task<WordSetDto> ConfirmProposedWordAsync(Guid userId, Guid proposedWordId);
        Task<bool> DeleteProposedWords(Guid proposedWordId);
        Task<List<WordSetDto>> ConfirmProposedWordsAsync(Guid userId, Guid[] proposedWordsId);
        Task<WordSetDto> UpdateWordSetAsync(Guid userId, WordSetUpdateRequest wordSetupdateRequset);
        Task<bool> DeleteWordSetAsync(Guid wordSetId);
        Task<bool> DeleteComplaintAsync(Guid cmplaintId);
    }
}
