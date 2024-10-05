using Airbnb.Application.Features.BookingToPayment.Entities;
using Airbnb.Application.Features.BookingToPayments.Command;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.BookingToPayment.Command
{
    public class BookingToPaymentCommandValidator : AbstractValidator<CreateCustomerBookingsCommand>
    {
        public BookingToPaymentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Booking).NotNull();
        }
    }
}
