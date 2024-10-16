using Airbnb.Application.Features.PaymentBooking.Command.BookingCancelation;
using Airbnb.Application.Features.PaymentBooking.Command.CreateBooking;
using Airbnb.Application.Features.PaymentBooking.Command.PayBooking;
using Airbnb.Application.Features.PaymentBooking.Command.RegisterBooking;
using Airbnb.Application.Features.PaymentBooking.Command.UpdateBooking;
using Airbnb.Application.Features.PaymentBooking.Query;
using Airbnb.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Airbnb.APIs.Controllers
{
	public class PaymentBookingController:APIBaseController
    {
        private readonly IMediator _mediator;

        public PaymentBookingController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet("GetBookingById/{bookingId}")]
        public async Task<ActionResult<Responses>> GetBookingById(string bookingId)
        {
            var query=new GetBookingQuery(bookingId);
            return Ok(await _mediator.Send(query));
        }

        [HttpPost("reateBooking")]
        public async Task<ActionResult<Responses>> CreateBooking(CreateBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpPut("UpdateBooking")]
        public async Task<ActionResult<Responses>> GetUserBookings(UpdateBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpDelete("DeleteBooking/{bookingId}")]
        public async Task<ActionResult<Responses>> BookingCancelation(string bookingId)
        {
            var command = new BookingCancelationCommand(bookingId);
            return Ok(await _mediator.Send(command));
        }

        [HttpPost("PayBooking")]
        public async Task<ActionResult<Responses>> PayBooking(string bookingId)
        {
            var command = new PayBookingCommand(bookingId);
            return Ok(await _mediator.Send(command));
        }
        [HttpPost("RegisterBooking")]
        public async Task<ActionResult<Responses>> RegisterBooking(RegisterBookingPaymentCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

    }
}
