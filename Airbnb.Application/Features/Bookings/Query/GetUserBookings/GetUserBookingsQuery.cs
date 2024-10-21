using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Specifications;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Query.GetUserBookings
{
	public record GetUserBookingsQuery : IRequest<Responses>
	{
		public string UserId { get; set; }
		public GetUserBookingsQuery(string userId)
		{
			UserId = userId;
		}
	}

	public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, Responses>
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		public GetUserBookingsQueryHandler(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
		}

		public async Task<Responses> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
		{
			var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			if (user == null || user.Id != request.UserId)
			{
				return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);
			}

			var spec = new BookingWithSpec(userId: request.UserId);
			var bookings = await _unitOfWork.Repository<Booking, int>().GetAllWithSpecAsync(spec)!;
			if (!bookings.Any())
			{
				return await Responses.FailurResponse($"User with Id {request.UserId} haven't bookings yet!", HttpStatusCode.NotFound);
			}
			var data = bookings.Adapt<List<GetUserBookingsDto>>();
			return await Responses.SuccessResponse(data);
		}
	}
}

