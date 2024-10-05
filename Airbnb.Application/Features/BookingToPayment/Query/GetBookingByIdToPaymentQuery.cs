using Airbnb.Application.Features.BookingToPayment.Entities;
using Airbnb.Application.Features.BookingToPayments.Command;
using Airbnb.Domain;
using Mapster;
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

            if (bookings.IsNull)
            {
                return await Responses.FailurResponse(new CreateCustomerBookingsCommand(request.Id), HttpStatusCode.NotFound);
            }
            else
            {
                var response = JsonSerializer.Deserialize<CustomerBookings>(bookings);
                //var maped = response.Adapt<CreateCustomerBookingsCommand>();
                return await Responses.SuccessResponse(response);
            }
        }
    }
}
