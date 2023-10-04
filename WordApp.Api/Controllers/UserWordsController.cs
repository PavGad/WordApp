using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WordApp.Domain.Interfaces;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.UserWordsDtos;

namespace WordApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserWordsController : ControllerBase
    {

        private readonly IUserWordService _userWordService;
        public UserWordsController(IUserWordService userWordService)
        {
            _userWordService = userWordService;
        }

        [HttpGet("get-userwords")]
        public async Task<List<UserWordDto>> GetUserWords()
        {
            var userId = GetUserIdFromRequest();
            return await _userWordService.GetUserWordsAsync(userId);
        }

        [HttpPost("add-userwords")]
        public async Task<IEnumerable<UserWordDto>> AddUserWords(UserWordRequest[] words)
        {
            var userId = GetUserIdFromRequest();
            return await _userWordService.AddUserWordsAsync(userId, words);
        }

        [HttpDelete("delete-userwords")]
        public async Task<bool> DeleteUserWords(Guid[] wordIds)
        {
            var userId = GetUserIdFromRequest();
            return await _userWordService.DeleteUserWordsAsync(userId, wordIds);
        }

        [HttpPut("update-userwords")]
        public async Task<IEnumerable<UserWordDto>> UpdateUserWords(UserWordDto[] words)
        {
            var userId = GetUserIdFromRequest();
            return await _userWordService.UpdateUserWordsAsync(userId, words);
        }

        [HttpPost("change-userword-stage")]
        public async Task<bool> ChangeUserWordStage(Guid wordId, int stage)
        {
            var userId = GetUserIdFromRequest();
            return await _userWordService.ChangeUserWordStageAsync(userId, wordId, stage);
        }

        [HttpGet("get-notification")]
        public async Task<UserWordsNotificationDto> GetUserWordsNotification()
        {
            var userId = GetUserIdFromRequest();
            return await _userWordService.GetUserWordsNotificationsAsync(userId);
        }

        private Guid GetUserIdFromRequest()
        {
            Console.WriteLine(DateTimeOffset.Now);
            var id = Request.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(id);  
        }
    }
}
