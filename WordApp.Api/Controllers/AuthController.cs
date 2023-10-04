using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using WordApp.Domain.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _config;
        private int RefreshTokenExpirationTimeMinute => _config.GetValue<int>("JwtSettings:RefreshTokenExpirationTimeMin");

        public AuthController(IAuthService authService, IConfiguration config)
        {
            _authService = authService;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("log-in")]
        public async Task<LogInResponse> LogIn(UserCredentials credentials)
        {
            var logInResponse = await _authService.LogInAsync(credentials);

            SetTokenCookie(logInResponse.RefreshToken);

            return logInResponse;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<SignUpResponse> SignUp(UserCredentials credentials)
        {
            var signUpResponse = await _authService.SignUpAsync(credentials);

            SetTokenCookie(signUpResponse.RefreshToken);

            return signUpResponse;
        }

        [HttpPost("log-out")]
        public async Task<bool> LogOut()
        {
            var token = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
            {
                throw new AuthenticationException("RefreshToken is required");
            }

            return await _authService.LogOutAsync(token);
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<LogInResponse> RefreshToken()
        {
            string? authHeader = Request.Headers["Authorization"];
            var accessToken = authHeader?.Remove(0, 7);
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new AuthenticationException("RefreshToken and AccessToken are required");
            }

            var logInResponse = await _authService.RefreshTokenAsync(accessToken, refreshToken);

            SetTokenCookie(logInResponse.RefreshToken);
            return logInResponse;
        }

        [AllowAnonymous]
        [HttpGet("check-username-uniqueness")]
        public async Task<bool> CheckUsernameUniqueness(string username)
        {
            return await _authService.CheckUsernameUniquenessAsync(username);
        }

        [AllowAnonymous]
        [HttpGet("env-test")]
        public int GetMinStageToCheckConfiguration()
        {

            int minStage = _config.GetValue<int>("ExerciseOptions:MinStage");
            return minStage;
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.Now.AddMinutes(RefreshTokenExpirationTimeMinute),
                SameSite = SameSiteMode.None,
                Secure = true,
                IsEssential = true,
                Path = "/",
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
    }
}
