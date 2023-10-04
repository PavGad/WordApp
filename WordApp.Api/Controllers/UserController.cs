using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordApp.Domain.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;
        public UserController(IUserService service)
        {
            _userService = service;
        }

        [AllowAnonymous]
        [HttpGet("check-username-uniqueness")]
        public async Task<bool> CheckUsernameUniqueness(string username)
        {
            return await _userService.CheckUsernameUniquenessAsync(username);
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("get-username-by-id")]
        public async Task<UserInfo> GetUserInfoById(string id)
        {
            return await _userService.GetUserInfoById(Guid.Parse(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("ban-user")]
        public async Task<IEnumerable<UserInfo>> BanUser(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
