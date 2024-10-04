using Airbnb.Domain;
using FluentValidation;
using MediatR;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using bookingToPayment = Airbnb.Application.Features.BookingToPayment.Entities.BookingToPayment;
namespace Airbnb.Application.Features.BookingToPayment.Command
{
    public record CreateCustomerBookingsCommand : IRequest<Responses>
    {
        public string Id { get; set; }
        public List<bookingToPayment> bookings { get; set; }
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
            var serialized = JsonSerializer.Serialize(request);
            var IsCreatedOrUpdated = await _database.StringSetAsync(request.Id, serialized, TimeSpan.FromDays(5));
            return (IsCreatedOrUpdated == true) ?
                 await Responses.SuccessResponse(request) :
                 await Responses.FailurResponse("Un Expected Error", HttpStatusCode.BadRequest);
        }
    }
}
