using Airbnb.Application.Features.Bookings.Command;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Booking;
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
    }
}
