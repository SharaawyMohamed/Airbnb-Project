using Airbnb.Application.Features.BookingToPayment.Command;
using Airbnb.Application.Features.BookingToPayment.Query;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
    public class BookingsToPaymentController:APIBaseController
    {
        private readonly IMediator _mediator;

        public BookingsToPaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateOrUpdateCustomerBookings")]
        public async Task<ActionResult<Responses>> CreateOrUpdateBooking(CreateCustomerBookingsCommand booking)
        {
            return Ok(await _mediator.Send(booking));
        }

        [HttpGet("GetCustomerBookings")]
        public async Task<ActionResult<Responses>> GetCustomerBookings(string Id)
        {
            var query=new GetBookingByIdToPaymentQuery(Id);
            return Ok(await _mediator.Send(query));
        }

        [HttpDelete("DeleteCustomerBookings")]
        public async Task<ActionResult<Responses>> DeleteCustomerBookings(string Id)
        {
            var query = new DeleteCustomerBookingsCommand(Id);
            return Ok(await _mediator.Send(query));
        }
    }
}
