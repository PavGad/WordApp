using System.Security.Authentication;
using System.Security.Claims;
using WordApp.Domain.Interfaces;
using WordApp.Persistence.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<SignUpResponse> SignUpAsync(UserCredentials userCredentials)
        {
            ArgumentNullException.ThrowIfNull(userCredentials);

            var isNameUnique = await _userRepository.CheckUsernameUniquenessAsync(userCredentials.Username);
            if (!isNameUnique)
            {
                throw new AuthenticationException($"User {userCredentials.Username} already exists");
            }

            userCredentials.Password = _passwordService.HashPassword(userCredentials.Password);

            var user = await _userRepository.CreateUserAsync(userCredentials);

            var accessToken = _jwtService.CreateToken(user);
            var refreshToken = await _userRepository.CreateRefreshTokenAsync(user);

            return new SignUpResponse(accessToken, refreshToken.Token, user.Id);
        }

        public async Task<LogInResponse> LogInAsync(UserCredentials userCredentials)
        {
            ArgumentNullException.ThrowIfNull(userCredentials);

            var user = await _userRepository.GetUserByUsernameAsync(userCredentials.Username);
            if (user == null || !_passwordService.CheckPassword(userCredentials.Password, user.Password))
            {
                throw new AuthenticationException($"Invalid username or password");
            }

            var accessToken = _jwtService.CreateToken(user);
            var refreshToken = await _userRepository.CreateRefreshTokenAsync(user);

            return new LogInResponse(accessToken, refreshToken.Token);
        }

        public async Task<bool> LogOutAsync(string refreshToken)
        {
            return await _userRepository.RevokeRefreshTokenAsync(refreshToken);
        }

        public async Task<LogInResponse> RefreshTokenAsync(string accessToken, string refreshToken)
        {
            var userId = _jwtService.DecodeToken(accessToken).Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();

            if (String.IsNullOrEmpty(userId))
            {
                throw new AuthenticationException("AccessToken is required");
            }

            var revokeResult = await _userRepository.RevokeRefreshTokenAsync(refreshToken);
            if (!revokeResult)
            {
                throw new AuthenticationException("Refresh token is required");
            }

            var id = Guid.Parse(userId);
            var user = await _userRepository.GetUserByIdAsync(id);

            var newRefreshToken = await _userRepository.CreateRefreshTokenAsync(user);
            var newAccessToken = _jwtService.CreateToken(user);

            return new LogInResponse(newAccessToken, newRefreshToken.Token);
        }

        public async Task<bool> CheckUsernameUniquenessAsync(string username)
        {
            return await _userRepository.CheckUsernameUniquenessAsync(username);
        }
    }
}
