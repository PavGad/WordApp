using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Persistence.Interfaces
{
    public interface IWordSetRepository
    {
        Task<WordSetDto> AddNewWordSet(Guid userId, string imageUrl, WordSetRequest wordSetRequest);
        Task<WordSet> GetWordSetByName(string name);
        Task<ComplaintDto> AddComplaint(Guid userId, ComplaintRequest newComplaintDto);
        Task<IEnumerable<ProposedWordDto>> AddProposedWord(Guid userId, ProposedWordsRequest proposedWordsRequest);
        Task<(List<WordSetDto>, int)> GetWordSets(WordSetFilterCriteria filterCriteria);

        Task<List<WordSetDto>> GetUnconfirmedWordSets();
        Task<WordSetDto> ConfirmWordSetAsync(Guid userId, Guid wordSetId);
        Task<WordSetDto> UpdateWordSetAsync(Guid userId, WordSetUpdateRequest wordSetupdateRequset);
        Task<bool> DeleteWordSetAsync(Guid wordSetId);

        Task<List<ProposedWordDto>> GetProposedWordsAsync();
        Task<WordSetDto> ConfirmProposedWordAsync(Guid usedId, Guid proposedWordId);
        Task<List<WordSetDto>> ConfirmProposedWordsAsync(Guid usedId, Guid[] proposedWordId);
        Task<bool> DeleteProposedWordAsync(Guid proposedWordId);

        Task<List<ComplaintDto>> GetComplaintsAsync();
        Task<bool> DeleteComplaintAsync(Guid complaintId);
    }
}
