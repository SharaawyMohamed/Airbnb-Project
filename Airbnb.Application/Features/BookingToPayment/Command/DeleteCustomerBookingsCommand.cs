using Airbnb.Domain;
using MediatR;
using StackExchange.Redis;
using System.Net;

namespace Airbnb.Application.Features.BookingToPayment.Command
{
    public record DeleteCustomerBookingsCommand : IRequest<Responses>
    {
        public string Id { get; set; }
        public DeleteCustomerBookingsCommand(string id)
        {
            Id = id;
        }
    }
    public class DeleteCustomerBookingsCommandHandler : IRequestHandler<DeleteCustomerBookingsCommand, Responses>
    {
        private readonly IDatabase _database;
        public DeleteCustomerBookingsCommandHandler(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<Responses> Handle(DeleteCustomerBookingsCommand request, CancellationToken cancellationToken)
        {
            var IsDeleted = await _database.KeyDeleteAsync(request.Id);
            return IsDeleted == true ?
                await Responses.SuccessResponse("Customer bookings has been deleted successfully.") :
                await Responses.FailurResponse("Internal server error.",HttpStatusCode.InternalServerError);
        }
    }
}
