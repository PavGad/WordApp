using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WordApp.Domain.Interfaces;
using WordApp.Shared.Dtos.WordSetDtos;

namespace WordApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WordSetController : ControllerBase
    {
        private readonly IWordSetService _wordSetService;

        public WordSetController(IWordSetService wordSetService)
        {
            _wordSetService = wordSetService;
        }

        [HttpPost("add-word-set")]
        public async Task<WordSetDto> AddWordSet(WordSetRequest wordSetRequest)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.AddWordSetAsync(userId, wordSetRequest);
        }

        [AllowAnonymous]
        [HttpPost("get-for-page")]
        public async Task<WordSetGetDto> GetWordSets(WordSetFilterCriteria filterCriteria)
        {
            return await _wordSetService.GetWordSetsAsync(filterCriteria);
        }

        [HttpGet("check-set-name-uniqueness")]
        public async Task<bool> CheckWordSetUniqueness(string? name)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.CheckWordSetUniquenesAsync(name);
        }

        [HttpPost("propose-words")]
        public async Task<IEnumerable<ProposedWordDto>> ProposeWords(ProposedWordsRequest proposedWordsRequest)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.ProposeWordsAsync(userId, proposedWordsRequest);
        }

        [HttpPost("complain")]
        public async Task<ComplaintDto> Complain(ComplaintRequest compalinRequest)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.ComplainAsync(userId, compalinRequest);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-complaints")]
        public async Task<IEnumerable<ComplaintDto>> GetComplaints()
        {
            return await _wordSetService.GetComplaintsAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-complaint")]
        public async Task<bool> DeleteComplaint(Guid complaintId)
        {
            return await _wordSetService.DeleteComplaintAsync(complaintId);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-proposed-words")]
        public async Task<IEnumerable<ProposedWordDto>> GetProposedWords()
        {
            return await _wordSetService.GetProposedWordsAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-unconfirmed-sets")]
        public async Task<IEnumerable<WordSetDto>> GetUnconfirmedSets()
        {
            return await _wordSetService.GetUnconfirmedWordSetsAsync();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("confirm-set")]
        public async Task<WordSetDto> ConfirmWordSet(ConfirmSetRequest confirmSetRequest)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.ConfirmWordSetAsync(userId, confirmSetRequest);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("confirm-proposed-word")]
        public async Task<WordSetDto> ConfirmProposedWord(Guid proposedWord)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.ConfirmProposedWordAsync(userId, proposedWord);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("confirm-proposed-words")]
        public async Task<List<WordSetDto>> ConfirmProposedWords(Guid[] proposedWord)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.ConfirmProposedWordsAsync(userId, proposedWord);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delte-proposed-word")]
        public async Task<bool> DeleteProposedWord(Guid proposedWord)
        {
            return await _wordSetService.DeleteProposedWords(proposedWord);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("update-word-set")]
        public async Task<WordSetDto> UpdateWordSet(WordSetUpdateRequest updateWordSetRequest)
        {
            var userId = GetUserIdFromRequest();
            return await _wordSetService.UpdateWordSetAsync(userId, updateWordSetRequest);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete-word-set")]
        public async Task<bool> DeleteWordSet(Guid wordSetId)
        {
            return await _wordSetService.DeleteWordSetAsync(wordSetId);
        }

        private Guid GetUserIdFromRequest()
        {
            var id = Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id);
        }
    }
}
