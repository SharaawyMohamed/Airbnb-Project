using Airbnb.Application.Features.BookingToPayment.Entities;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Booking;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Repositories;
using FluentValidation;
using Mapster;
using MediatR;
using StackExchange.Redis;
using System.Net;
using System.Text.Json;
using bookingToPayment = Airbnb.Application.Features.BookingToPayment.Entities.BookingToPayment;
namespace Airbnb.Application.Features.BookingToPayments.Command
{
    public record CreateCustomerBookingsCommand : IRequest<Responses>
    {
        public string Id { get; set; }
        public BookingToPaymentDto Booking { get; set; }
        public CreateCustomerBookingsCommand(string id)
        {
            Id = id;
        }
    }
    public class CreateCustomerBookingCommandHandler : IRequestHandler<CreateCustomerBookingsCommand, Responses>
    {
        private readonly IDatabase _database;
        private readonly IValidator<CreateCustomerBookingsCommand> _validator;
        public CreateCustomerBookingCommandHandler(IConnectionMultiplexer redis, IValidator<CreateCustomerBookingsCommand> validator)
        {
            _database = redis.GetDatabase();
            _validator = validator;
        }

        public async Task<Responses> Handle(CreateCustomerBookingsCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return await Responses.FailurResponse(validation.Errors, HttpStatusCode.BadRequest);
            }

            var maped = request.Adapt<CustomerBookings>();
            var serialized = JsonSerializer.Serialize(maped);

            var IsCreatedOrUpdated = await _database.StringSetAsync(request.Id, serialized, TimeSpan.FromDays(5));

            //TODO: SendNotification here for user where he create booking to warning if he not pay money in period 5 days booking will be cancled
           
            if (!IsCreatedOrUpdated)
                return await Responses.FailurResponse("Un Expected Error", HttpStatusCode.BadRequest);

            var customerBookings = await _database.StringGetAsync(request.Id);
            var response = JsonSerializer.Deserialize<CustomerBookings>(customerBookings);
            //var mapedResponse = response.Adapt<dto>();
            return await Responses.SuccessResponse(response);
        }
    }
}
