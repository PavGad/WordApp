using NSubstitute;
using System.Security.Authentication;
using WordApp.Domain.Interfaces;
using WordApp.Domain.Services;
using WordApp.Persistence.Interfaces;
using WordApp.Persistence.Models;
using WordApp.Shared.Dtos.AuthDtos;
using Xunit;

namespace UnitTests.ServiceTests.AuthServiceTests
{
    public class LogInAsyncTests
    {
        [Fact]
        public async Task LogInAsyncAsyncThrowsIfCredentialsIsNull()
        {
            // Arrange
            var mockIUserRepository = Substitute.For<IUserRepository>();
            var mockIJwtService = Substitute.For<IJwtService>();
            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await authService.LogInAsync(null));
        }

        [Theory]
        [InlineData("username", "password")]
        public async Task LogInAsyncThrowsIfUserNotExist(string username, string password)
        {
            // Arrange
            var userCredentials = new UserCredentials() { Username = username, Password = password };

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.GetUserByUsernameAsync(userCredentials.Username).Returns<UserDto>(x=>null);

            var mockIJwtService = Substitute.For<IJwtService>();
            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await authService.LogInAsync(userCredentials));
        }

        [Theory]
        [InlineData("username", "password")]
        public async Task LogInAsyncThrowsIfInvalidPassword(string username, string password)
        {
            // Arrange
            var userCredentials = new UserCredentials() { Username = username, Password = password };
            var userDto = new UserDto() { Id = Guid.NewGuid(), Password = "12345", Username = username, Role = WordApp.Shared.Enums.Role.User };

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.GetUserByUsernameAsync(userCredentials.Username).Returns<UserDto>(userDto);

            var mockIJwtService = Substitute.For<IJwtService>();
            var mockIPasswordService = Substitute.For<IPasswordService>();
            mockIPasswordService.CheckPassword(password, userDto.Password).Returns(false);

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await authService.LogInAsync(userCredentials));
        }

        [Theory]
        [InlineData("username", "password")]
        public async Task LogInAsyncReturnsLogInResponse(string username, string password)
        {
            // Arrange
            string refreshToken = "Refresh-token";
            string accessToken  = "Access-token";

            var userCredentials = new UserCredentials() { Username = username, Password = password };
            var userDto = new UserDto() { Id = Guid.NewGuid(), Password = "12345", Username = username, Role = WordApp.Shared.Enums.Role.User };
            var refreshTokenDto = new RefreshTokenDto() { Token = refreshToken, ExpiresOn = DateTimeOffset.Now };

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.GetUserByUsernameAsync(userCredentials.Username).Returns<UserDto>(userDto);
            mockIUserRepository.CreateRefreshTokenAsync(userDto).Returns(refreshTokenDto);

            var mockIJwtService = Substitute.For<IJwtService>();
            mockIJwtService.CreateToken(userDto).Returns(accessToken);

            var mockIPasswordService = Substitute.For<IPasswordService>();
            mockIPasswordService.CheckPassword(password, userDto.Password).Returns(true);

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            //Act

            var logInResult = await authService.LogInAsync(userCredentials);
            
            // Assert
            Assert.Equal(refreshTokenDto.Token, logInResult.RefreshToken);
            Assert.Equal(accessToken, logInResult.AccessToken);
        }
    }
}
