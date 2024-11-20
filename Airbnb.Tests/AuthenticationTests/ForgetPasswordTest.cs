using Airbnb.Application.Services;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Tests.FakeObjects;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Tests.AuthenticationTests
{
	public class ForgetPasswordTest:TestBase
	{
		[Fact]
		public async Task ForgetPassword_WhenForgetPasswordExecutedSuccessfully_ShouldBeReturnSuccessResponse()
		{
			// Arrange
			var _userManager=GetUserManager();
			var user = new FakeAccount().Generate();
			await _userManager.CreateAsync(user);

			var _mailService = new Mock<IMailService>();
			_mailService.Setup(x => x.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),null)).Returns(Task.CompletedTask);

			var _cacheMemory = _serviceProvider.GetRequiredService<IMemoryCache>();
			var handler = new UserService(_userManager, null, null, _mailService.Object, null, _cacheMemory, null, null, null);
			// Act
			var result = await handler.ForgetPassword(new ForgetPasswordDto { Email = user.Email! });

			// Assert
			result.IsSuccess.Should().BeTrue();

		}

		[Fact]
		public async Task ForgetPassword_WhenEmailDoesNotExist_ShouldReturnFailureResponse()
		{
			// Arrange
			var _userManager = GetUserManager();

			var _mailService = new Mock<IMailService>();
			var _cacheMemory = _serviceProvider.GetRequiredService<IMemoryCache>();
			var handler = new UserService(_userManager, null, null, _mailService.Object, null, _cacheMemory, null, null, null);

			// Act
			var result = await handler.ForgetPassword(new ForgetPasswordDto { Email = "notfound@gmail.com" });

			// Assert
			result.IsSuccess.Should().BeFalse();
			result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

	}
}
