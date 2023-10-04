using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using WordApp.Persistence.Interfaces;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.AuthDtos;
using WordApp.Shared.Enums;

namespace WordApp.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public int RefreshTokenExpirationTimeMinute => _config.GetValue<int>("JwtSettings:RefreshTokenExpirationTimeMin");

        public UserRepository(DataContext context, IConfiguration config, IMapper mapper)
        {
            this.context = context;
            _config = config;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(UserCredentials request)
        {
            User user = new User() { Username = request.Username, Password = request.Password, Role = Role.User };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<RefreshTokenDto> CreateRefreshTokenAsync(UserDto userDto)
        {
            var user = await context.Users
                .Where(u => u.Id == userDto.Id)
                .FirstOrDefaultAsync();

            var refreshToken = GenerateRefreshToken(user);

            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            return _mapper.Map<RefreshTokenDto>(refreshToken);
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            var token = await context.RefreshTokens
                .Where(t => t.Token == refreshToken)
                .FirstOrDefaultAsync();

            if (token == null || token.ExpiresOn < DateTimeOffset.Now)
            {
                return false;
            }

            token.ExpiresOn = DateTimeOffset.Now;
            return await context.SaveChangesAsync() >= 0;
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            return _mapper.Map<UserDto>(
                await context.Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync());
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            return _mapper.Map<UserDto>(
                await context.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync());
        }

        public async Task<bool> CheckUsernameUniquenessAsync(string username)
        {
            return await context.Users
                .Where(u => u.Username == username)
                .FirstOrDefaultAsync() == null;
        }

        private RefreshToken GenerateRefreshToken(User user)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiresOn = DateTimeOffset.Now.AddMinutes(RefreshTokenExpirationTimeMinute),
                    CreatedOn = DateTimeOffset.Now,
                    User = user
                };
            }
        }
    }
}
