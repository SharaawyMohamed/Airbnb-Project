using Airbnb.Application.Features.Notifications.Query.CheckNewNotifications;
using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Specifications;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications.Query.GetAllNotificationsWithpagination
{
	public class GetPaginatedNotificationsQuery : IRequest<Responses>
	{
		public string userId { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
	}
	public class GetPaginatedNotificationsQueryHandler : IRequestHandler<GetPaginatedNotificationsQuery, Responses>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IHttpContextAccessor _contextAccessor;

		public GetPaginatedNotificationsQueryHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper, IHttpContextAccessor contextAccessor)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_mapper = mapper;
			_contextAccessor = contextAccessor;
		}

		public async Task<Responses> Handle(GetPaginatedNotificationsQuery request, CancellationToken cancellationToken)
		{
			var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			if (user == null)
			{
				return await Responses.FailurResponse("UnAuthorized User", HttpStatusCode.Unauthorized);
			}
			var notifParams = new NotificationParam
			{
				userId = user.Id,
				pageIndex = request.PageIndex,
				PageSize = request.PageSize
			};

			var spec = new NotificationsWithSpeck(notifParams);
			var notifications = await _unitOfWork.Repository<Notification, int>().GetAllWithSpecAsync(spec)!;
			return await Responses.SuccessResponse(_mapper.Map<IEnumerable<GetNewNotificationsQueryDto>>(notifications));
		}
	}
}
