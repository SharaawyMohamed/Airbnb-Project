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
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Notifications.Query.CheckNewNotifications
{
	public record GetNewNotificationsQuery:IRequest<Responses>;
	internal class GetNewNotificationsHandler : IRequestHandler<GetNewNotificationsQuery, Responses>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IMapper _mapper;
		public GetNewNotificationsHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_mapper = mapper;
		}

		public async Task<Responses> Handle(GetNewNotificationsQuery request, CancellationToken cancellationToken)
		{
			var user=await GetUser.GetCurrentUserAsync(_contextAccessor,_userManager);
			if(user== null)
			{
				return await Responses.FailurResponse("User not found!", HttpStatusCode.Unauthorized);
			}
			var spec=new NotificationsWithSpeck(user.Id,false);
			var GetUserNotificationsNotRead=await _unitOfWork.Repository<Notification,int>().GetAllWithSpecAsync(spec)!;
			var maped = _mapper.Map<List<GetNewNotificationsQueryDto>>(GetUserNotificationsNotRead);
			return await Responses.SuccessResponse(maped);

		}
	}

}
