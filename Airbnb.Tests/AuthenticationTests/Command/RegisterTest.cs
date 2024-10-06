using Airbnb.Application.Services;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Tests.FakeObjects;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Tests.AuthenticationTests.Command
{
    public class RegisterTest:TestBase
    {
        [Fact]
        public async Task ShouldBeReturnSuccessResponse_WhenRegistrationDoneSuccessfully()
        {
            //Arrange
            var _userManager = GetUserManager();
            var _signInManager = GetSignInManager();
            var _authService = new Mock<IAuthService>();
            _authService.Setup(x => x.CreateTokenAsync(It.IsAny<AppUser>(), It.IsAny<UserManager<AppUser>>())).ReturnsAsync("Token");
            var _mailService = new Mock<IMailService>();
            _mailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IList<IFormFile>>())).Returns(Task.CompletedTask);

            var _cache = _serviceProvider.GetRequiredService<IMemoryCache>();
            var _mapper = _serviceProvider.GetRequiredService<IMapper>();
            var _unitOfWork = GetUnitOfWork();

            var user = new FakeRegisterDto().Generate();
            user.Password = "User@123";

            var handler = new UserService(
                _userManager,
                _signInManager,
                _authService.Object,
                _mailService.Object,
                _mapper,
                _cache,
                _unitOfWork
                );
            // Act
            var result=await handler.Register(user);

            //Assert
            result.IsSuccess.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}
