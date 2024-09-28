using Airbnb.Application.Features.Bookings.Command;
using Airbnb.Application.Features.Bookings.Query;
using Airbnb.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
    public class BookingController:APIBaseController
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateBooking")]
        public async Task<ActionResult<Responses>> CreateBooking(CreateBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet("GetBookings/{userId}")]
        public async Task<ActionResult<Responses>> GetUserBookings(string userId)
        {
            var query=new GetUserBookingsQuery(userId);
            return Ok(await _mediator.Send(query));
        }

    }
}
