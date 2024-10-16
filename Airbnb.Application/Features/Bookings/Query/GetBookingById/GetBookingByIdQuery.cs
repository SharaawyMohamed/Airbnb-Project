using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Query.GetBookingById
{
	public class GetBookingByIdQuery:IRequest<Responses>
	{
		public int BookingId { get; set; }
        public GetBookingByIdQuery(int bookingId)
        {
            BookingId = bookingId;
        }
    }

	public class GetBookingByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Responses>
	{
		private readonly IUnitOfWork _unitOfWork;

		public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task<Responses> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
		{
			var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(request.BookingId)!;
			if (booking == null) return await Responses.FailurResponse($"InValid booking id {request.BookingId}", HttpStatusCode.NotFound);

			return await Responses.SuccessResponse(booking.Adapt<GetBookingByIdDto>());
		}
	}
}
