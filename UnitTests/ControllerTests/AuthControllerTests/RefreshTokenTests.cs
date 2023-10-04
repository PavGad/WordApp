using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.Security.Authentication;
using WordApp.Api.Controllers;
using WordApp.Domain.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;
using Xunit;

namespace UnitTests.ControllerTests.AuthControllerTests
{
    public class RefreshTokenTests
    {
        [Theory]
        [InlineData("Bearer=access-token")]
        public async Task RefreshTokenThrowsIfRefreshTokenCookiesNotFound(string auth)
        {
            // Arrange
            var mockAuthService = Substitute.For<IAuthService>();
            var configuration = Substitute.For<IConfiguration>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Authorization"] = auth;
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await controller.RefreshToken());
        }

        [Theory]
        [InlineData("refresh-token")]
        public async Task RefreshTokenThrowsIfAuthHeaderNotFound(string refresh)
        {
            // Arrange
            var mockAuthService = Substitute.For<IAuthService>();
            var configuration = Substitute.For<IConfiguration>();

            var httpContext = new DefaultHttpContext();
            var newCookies = new[] { $"refreshToken={refresh}" };
            httpContext.Request.Headers["Cookie"] = newCookies;

            //httpContext.Request.
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await controller.RefreshToken());
        }

        [Theory]
        [InlineData("refresh-token", "access-token")]
        public async Task RefreshTokenExtractsRefreshTokenCookieAndAccessTokenAndPassThemToService(string refresh, string access)
        {
            // Arrange
            var mockAuthService = Substitute.For<IAuthService>();
            LogInResponse loginResponse = new LogInResponse("new-access-token", "new-refresh-token");
            mockAuthService.RefreshTokenAsync(access, refresh).Returns(loginResponse);

            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:RefreshTokenExpirationTimeMin", "10"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var httpContext = new DefaultHttpContext();
            var newCookies = new[] { $"refreshToken={refresh}" };
            httpContext.Request.Headers["Cookie"] = newCookies;
            httpContext.Request.Headers["Authorization"] = $"Bearer={access}";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            //Act
            var result = await controller.RefreshToken();

            // Assert
            await mockAuthService.Received().RefreshTokenAsync(Arg.Is<String>(p => p == access), Arg.Is<String>(x=>x==refresh));
            Assert.Equal(loginResponse, result);
        }

        [Theory]
        [InlineData("refresh-token", "access-token")]
        public async Task RefreshTokenSetRefreshTokenCookie(string refresh, string access)
        {
            // Arrange
            var mockAuthService = Substitute.For<IAuthService>();
            LogInResponse loginResponse = new LogInResponse("new-access-token", "new-refresh-token");
            mockAuthService.RefreshTokenAsync(access, refresh).Returns(loginResponse);

            var inMemorySettings = new Dictionary<string, string> {
                {"JwtSettings:RefreshTokenExpirationTimeMin", "10"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var httpContext = new DefaultHttpContext();
            var newCookies = new[] { $"refreshToken={refresh}" };
            httpContext.Request.Headers["Cookie"] = newCookies;
            httpContext.Request.Headers["Authorization"] = $"Bearer={access}";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            //Act
            var result = await controller.RefreshToken();

            // Assert
            var refreshTokenCookie = httpContext.Response.GetTypedHeaders().SetCookie.Where(x => x.Name == "refreshToken").FirstOrDefault();
            Assert.NotNull(refreshTokenCookie);
            Assert.Equal(result.RefreshToken, refreshTokenCookie.Value);
        }
    }
}
