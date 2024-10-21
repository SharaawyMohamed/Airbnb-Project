using Airbnb.Application.Features.Bookings.Query.GetUserBookings;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Specifications;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Query.GetAllBookings
{
	public record GetAllBookingsQuery:IRequest<Responses>;
	public class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, Responses>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetAllBookingsQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Responses> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
		{
			var spec = new BookingWithSpec();
			var bookings = await _unitOfWork.Repository<Booking, int>().GetAllWithSpecAsync(spec)!;

			if (!bookings.Any()) return await Responses.FailurResponse("There is no bookings yet!");

			return await Responses.SuccessResponse(bookings.Adapt<List<GetUserBookingsDto>>());
		}
	}
}
