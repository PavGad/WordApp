using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.Security.Authentication;
using WordApp.Api.Controllers;
using WordApp.Domain.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;
using Xunit;

namespace UnitTests.ControllerTests.AuthControllerTests
{
    public class SignUpTests
    {
        [Fact]
        public async Task SignUpThrowsIfCredentialsIsNull()
        {
            // Arrange
            var mockIconfiguration = Substitute.For<IConfiguration>();
            var mockAuthService = Substitute.For<IAuthService>();
            mockAuthService.SignUpAsync(null).Throws(new ArgumentNullException());

            var controller = new AuthController(mockAuthService, mockIconfiguration);

            // Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await controller.SignUp(null));
        }

        [Fact]
        public async Task SignUpThrowsIfCredentialsIsInvalid()
        {
            // Arrange
            UserCredentials invalidCredentials = new UserCredentials() { Username = "username", Password = "password" };
            var mockIconfiguration = Substitute.For<IConfiguration>();
            var mockAuthService = Substitute.For<IAuthService>();
            mockAuthService.SignUpAsync(invalidCredentials).Throws(new AuthenticationException());

            var controller = new AuthController(mockAuthService, mockIconfiguration);

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await controller.SignUp(invalidCredentials));
        }

        [Fact]
        public async Task SignUpWithValidCredentialsReturnsLoginResponse()
        {
            // Arrange
            UserCredentials validCredentials = new UserCredentials() { Username = "username", Password = "password" };
            SignUpResponse response = new SignUpResponse("AccessToken", "RefreshToken", new Guid());

            var mockAuthService = Substitute.For<IAuthService>();
            mockAuthService.SignUpAsync(validCredentials).Returns(response);

            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:RefreshTokenExpirationTimeMin", "10"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            //Act
            var result = await controller.SignUp(validCredentials);

            // Assert
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task LogInWithValidCredentialsSetRefreshTokenCookies()
        {
            // Arrange
            UserCredentials validCredentials = new UserCredentials() { Username = "username", Password = "password" };
            SignUpResponse response = new SignUpResponse("AccessToken", "RefreshToken", new Guid());

            var mockAuthService = Substitute.For<IAuthService>();
            mockAuthService.SignUpAsync(validCredentials).Returns(response);

            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:RefreshTokenExpirationTimeMin", "10"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            //Act
            var result = await controller.SignUp(validCredentials);

            // Assert
            var refreshTokenCookie = httpContext.Response.GetTypedHeaders().SetCookie.Where(x => x.Name == "refreshToken").FirstOrDefault();
            Assert.NotNull(refreshTokenCookie);
            Assert.Equal(result.RefreshToken, refreshTokenCookie.Value);
        }
    }
}
