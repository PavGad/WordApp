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
    public class LogOutTests
    {
        [Fact]
        public async Task LogOutThrowsIfRefreshTokenCookiesNotFound()
        {
            // Arrange
            var mockAuthService = Substitute.For<IAuthService>();
            var configuration = Substitute.For<IConfiguration>();

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            // Assert
            await Assert.ThrowsAsync<AuthenticationException>(async () => await controller.LogOut());
        }

        [Theory]
        [InlineData("123456789")]
        public async Task LogOutExtractsRefreshTokenCookieAndPassItToService(string refresh)
        {
            // Arrange
            var mockAuthService = Substitute.For<IAuthService>();
            mockAuthService.LogOutAsync(refresh).Returns(true);
            var configuration = Substitute.For<IConfiguration>();

            var httpContext = new DefaultHttpContext();
            var newCookies = new[] { $"refreshToken={refresh}" };
            httpContext.Request.Headers["Cookie"] = newCookies;

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AuthController(mockAuthService, configuration) { ControllerContext = controllerContext };

            //Act
            var result = await controller.LogOut();

            // Assert
            await mockAuthService.Received().LogOutAsync(Arg.Is<String>(p => p == refresh));
            Assert.True(result);
        }
    }
}
