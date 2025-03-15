using Airbnb.APIs.Controllers;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain;
using Airbnb.Tests.FakeObjects;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Airbnb.Tests.UsersTests
{
	public class GetAllUsersTest : TestBase
	{
		[Fact]
		public async Task GetAllUsers_WhenUsersReturnedSuccessfully_ShouldReturnSuccessResponse()
		{
			// Arrange
			var _userManager = GetUserManager();
			var users = new FakeAccount().Generate(5);
			foreach (var u in users)
			{
				await _userManager.CreateAsync(u, "Pa$$w0rd");
			}

			var configurationMock = new Mock<IConfiguration>();
			configurationMock.Setup(config => config["SomeKey"]).Returns("SomeValue");

			var _mapper = _serviceProvider.GetRequiredService<IMapper>();
			var handler = new UsersController(_userManager, null, null, null, _mapper);

			//Act
			var result = await handler.GetAllUsers();

			//Assert
			result.Result.Should().BeOfType<OkObjectResult>();
			var okResult = result.Result as OkObjectResult;
			var responseContent = okResult.Value as Responses;

			responseContent.Should().NotBeNull();
			var returnedUsers = responseContent.Data as List<UserDTO>;
			returnedUsers.Should().HaveCount(users.Count());
		}

	}
}
