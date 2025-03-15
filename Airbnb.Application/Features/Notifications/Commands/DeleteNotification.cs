using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications.Commands
{
	public class DeleteNotificationCommand : IRequest<Responses>
	{
		public int Id { get; set; }
		public string userId { get; set; }
	}
	public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Responses>
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteNotificationCommandHandler(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
		{
			_userManager = userManager;
			_unitOfWork = unitOfWork;
		}

		public async Task<Responses> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.userId);
			if (user == null)
			{
				return await Responses.FailurResponse("Invalid UserId", HttpStatusCode.NotFound);
			}

			var notification = await _unitOfWork.Repository<Notification, int>().GetByIdAsync(request.Id)!;
			if (notification == null)
			{
				return await Responses.FailurResponse($"Invalid Notification Id`{request.Id}`.", HttpStatusCode.NotFound);
			}

			_unitOfWork.Repository<Notification, int>().Remove(notification);
			await _unitOfWork.CompleteAsync();

			return await Responses.SuccessResponse($"Notification with `{request.Id}` has been removed.");
		}
	}
}
