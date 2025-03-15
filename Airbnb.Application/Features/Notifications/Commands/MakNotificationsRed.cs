using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications.Commands
{
	public class MakNotificationRed:IRequest<Responses>
	{
		public int Id { get; set; }
		public string userId { get; set; }
	}
	public class MakNotificationsRedHandler : IRequestHandler<MakNotificationRed, Responses>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		public MakNotificationsRedHandler(IUnitOfWork unitOfWork, UserManager<AppUser> usermanager)
		{
			_unitOfWork = unitOfWork;
			_userManager = usermanager;
		}

		public async Task<Responses> Handle(MakNotificationRed request, CancellationToken cancellationToken)
		{
			var user = await _userManager.FindByIdAsync(request.userId);
			if (user == null)
			{
				return await Responses.FailurResponse("UnAuthorized User", HttpStatusCode.Unauthorized);
			}

			var notification=await _unitOfWork.Repository<Notification,int>().GetByIdAsync(request.Id)!;
			if (notification == null)
			{
				return await Responses.FailurResponse("Notification not found!",HttpStatusCode.NotFound);
			}

			if(notification.UserId != user.Id)
			{
				return await Responses.FailurResponse("UnAuthorized User", HttpStatusCode.Unauthorized);
			}

			notification.IsRead = true;
			await _unitOfWork.CompleteAsync();
			return await Responses.SuccessResponse("Notification has been marked as read!");
		}
	}
}
