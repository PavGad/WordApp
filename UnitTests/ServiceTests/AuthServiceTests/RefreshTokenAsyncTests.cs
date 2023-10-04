using NSubstitute;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using WordApp.Domain.Interfaces;
using WordApp.Domain.Services;
using WordApp.Persistence.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;
using Xunit;

namespace UnitTests.ServiceTests.AuthServiceTests
{
    public class RefreshTokenAsyncTests
    {
        private string validJwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjUwOGFkNTIyLWU2MjktNDgyYS1iMTllLTNlY2EwZDZiYTQ3NSJ9.Kra14yhQg1wPQ014BsmWwiVa8PCKbbLMCeYqqBbPdcI";
        private string invalidJwtToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.e30.yXvILkvUUCBqAFlAv6wQ1Q-QRAjfe3eSosO949U73Vo";

        [Theory]
        [InlineData("acess", "refresh")]
        public async Task RefreshTokenAsyncThrowsIfAcessTokenInvalid(string accessToken, string refreshToken)
        {
            // Arranges
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(invalidJwtToken);

            var mockIUserRepository = Substitute.For<IUserRepository>();

            var mockIJwtService = Substitute.For<IJwtService>();
            mockIJwtService.DecodeToken(accessToken).Returns(jwtSecurityToken);

            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await authService.RefreshTokenAsync(accessToken, refreshToken));
        }

        [Theory]
        [InlineData("acess", "refresh")]
        public async Task RefreshTokenAsyncThrowsIfRefreshTokenNotExist(string accessToken, string refreshToken)
        {
            // Arranges
            //Guid id = new Guid("508ad522-e629-482a-b19e-3eca0d6ba475");
            var userDto = new UserDto();
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(validJwtToken);

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.RevokeRefreshTokenAsync(refreshToken).Returns(false);

            var mockIJwtService = Substitute.For<IJwtService>();
            mockIJwtService.DecodeToken(accessToken).Returns(jwtSecurityToken);

            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await authService.RefreshTokenAsync(accessToken, refreshToken));
        }

        [Theory]
        [InlineData("acess", "refresh")]
        public async Task RefreshTokenAsyncReturnsLogInResponseSuccess(string accessToken, string refreshToken)
        {
            // Arranges
            var newRefreshToken = "New Refresh Token";
            var newAccessToken = "New Access Token";
            Guid id = new Guid("508ad522-e629-482a-b19e-3eca0d6ba475");
            var userDto = new UserDto() { Id = id };
            var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(validJwtToken);
            var refToken = new RefreshTokenDto() { Token = newRefreshToken };

            var mockIUserRepository = Substitute.For<IUserRepository>();
            mockIUserRepository.RevokeRefreshTokenAsync(refreshToken).Returns(true);
            mockIUserRepository.GetUserByIdAsync(id).Returns(userDto);
            mockIUserRepository.CreateRefreshTokenAsync(userDto).Returns(refToken);

            var mockIJwtService = Substitute.For<IJwtService>();
            mockIJwtService.DecodeToken(accessToken).Returns(jwtSecurityToken);
            mockIJwtService.CreateToken(userDto).Returns(newAccessToken);

            var mockIPasswordService = Substitute.For<IPasswordService>();

            var authService = new AuthService(mockIUserRepository, mockIJwtService, mockIPasswordService);

            //Act
            var refreshResult = await authService.RefreshTokenAsync(accessToken, refreshToken);

            // Assert
            Assert.Equal(newRefreshToken, refreshResult.RefreshToken);
            Assert.Equal(newAccessToken, refreshResult.AccessToken);
        }
    }
}
