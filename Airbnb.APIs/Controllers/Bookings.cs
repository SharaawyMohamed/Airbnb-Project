using Airbnb.Application.Features.Bookings.Query.GetAllBookings;
using Airbnb.Application.Features.Bookings.Query.GetBookingById;
using Airbnb.Application.Features.Bookings.Query.GetUserBookings;
using Airbnb.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
	public class Bookings:APIBaseController
	{
		private readonly IMediator _mediator;

		public Bookings(IMediator mediator)
		{
			_mediator = mediator;
		}

		[Authorize(Roles ="Customer")]
		[HttpGet("GetUserBookings")]
		public async Task<ActionResult<Responses>> GetUserBookings(string userId)
		{
			var query = new GetUserBookingsQuery(userId);
			return Ok(await _mediator.Send(query));
		}	 
		[Authorize(Roles = "Customer")]
		[HttpGet("GetBookingById")]
		public async Task<ActionResult<Responses>> GetBookingById(int bookingId)
		{
           var query=new GetBookingByIdQuery(bookingId);
			return Ok(await _mediator.Send(query));
		}

		[Authorize(Roles ="Admin")]
		[HttpGet("GetAllBookings")]
		public async Task<ActionResult<Responses>> GetAllBookings()
		{
			return Ok(await _mediator.Send(new GetAllBookingsQuery()));
		}
	}

}
