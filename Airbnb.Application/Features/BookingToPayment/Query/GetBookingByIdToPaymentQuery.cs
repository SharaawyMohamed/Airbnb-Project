using Airbnb.Application.Features.BookingToPayment.Entities;
using Airbnb.Domain;
using MediatR;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;


namespace Airbnb.Application.Features.BookingToPayment.Query
{
    public record GetBookingByIdToPaymentQuery : IRequest<Responses>
    {
        public string Id { get; set; }
        public GetBookingByIdToPaymentQuery(string id)
        {
            Id = id;
        }
    }
    public class GetAllBookingsToPaymentQueryHandler : IRequestHandler<GetBookingByIdToPaymentQuery, Responses>
    {
        private readonly IDatabase _database;

        public GetAllBookingsToPaymentQueryHandler(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<Responses> Handle(GetBookingByIdToPaymentQuery request, CancellationToken cancellationToken)
        {
            var bookings = await _database.StringGetAsync(request.Id);
            return bookings.IsNull ?
                await Responses.FailurResponse(new CustomerBookings(request.Id) ,HttpStatusCode.NotFound) :
                await Responses.SuccessResponse(JsonSerializer.Deserialize<CustomerBookings>(bookings));
        }
    }
}
