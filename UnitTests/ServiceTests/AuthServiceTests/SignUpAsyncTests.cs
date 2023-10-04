using NSubstitute;
using System.Security.Authentication;
using WordApp.Domain.Interfaces;
using WordApp.Domain.Services;
using WordApp.Persistence.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;
using Xunit;

namespace UnitTests.ServiceTests.AuthServiceTests
{
    public class SignUpAsyncTests
    {
        [Fact]
        public async Task SignUpAsyncThrowsIfCredentialsIsNull()
        {
            // Arrange
            var mockIUserRepository = Substitute.For<IUserRepository>();
            var mockIJwtService = Substitute.For<IJwtService>();
            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await authService.SignUpAsync(null));
        }

        [Theory]
        [InlineData("username","password")]
        public async Task SignUpAsyncThrowsIfUsernameAlreadyExists(string username, string password)
        {
            // Arrange
            var userCredentials = new UserCredentials() { Username = username, Password = password };

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.CheckUsernameUniquenessAsync(userCredentials.Username).Returns(false);

            var mockIJwtService = Substitute.For<IJwtService>();
            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await authService.SignUpAsync(userCredentials));
        }

        [Theory]
        [InlineData("username", "password")]
        public async Task SignUpAsyncReturnsSignUpResponse(string username, string password)
        {
            // Arrange
            var userCredentials = new UserCredentials() { Username = username, Password = password };
            var userDto = new UserDto() { Id = Guid.NewGuid(), Password = "", Username = username, Role = WordApp.Shared.Enums.Role.User  };
            var refreshToken = new RefreshTokenDto() { Token = "refreshToken", ExpiresOn = DateTimeOffset.Now };

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.CheckUsernameUniquenessAsync(userCredentials.Username).Returns(true);
            mockIUserRepository.CreateUserAsync(userCredentials).Returns(userDto);
            mockIUserRepository.CreateRefreshTokenAsync(userDto).Returns(refreshToken);

            var mockIJwtService = Substitute.For<IJwtService>();
            mockIJwtService.CreateToken(userDto).Returns("Access-token");

            var mockIPasswordService = Substitute.For<IPasswordService>();
            mockIPasswordService.HashPassword(password).Returns("hashed-password");

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            //Act
            var res = await authService.SignUpAsync(userCredentials);

            // Assert
            Assert.Equal("Access-token", res.AccessToken);
            Assert.Equal(userDto.Id, res.Id);
            Assert.Equal(refreshToken.Token, res.RefreshToken);
        }
    }
}
