using Airbnb.Application.Features.Bookings.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Booking.Command
{
    public class CreateBookingCommandValidator:AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.BookingType).NotNull();
            RuleFor(x => x.PropertyId).NotNull();
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.StartDate).NotNull();
            RuleFor(x => x.EndDate).NotNull();
            RuleFor(x => x.PaymentMethod).NotNull();
        }
    }
}
