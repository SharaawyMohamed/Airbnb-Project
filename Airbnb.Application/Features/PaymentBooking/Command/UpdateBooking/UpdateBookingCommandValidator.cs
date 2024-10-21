using Airbnb.Domain.Entities;
using FluentValidation;

namespace Airbnb.Application.Features.PaymentBooking.Command.UpdateBooking
{
	public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
	{
		public UpdateBookingCommandValidator()
		{
			RuleFor(x => x.BookingId).NotNull();
			RuleFor(x => x.PaymentMethod).NotNull().Must(method => method == PaymentMethod.Cache || method == PaymentMethod.Visa);
			RuleFor(x => x.BookingType).NotNull();
		}
	}
}
