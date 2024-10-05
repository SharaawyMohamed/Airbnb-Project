using Airbnb.Application.Features.BookingToPayment.Entities;
using Airbnb.Application.Features.BookingToPayments.Command;
using Airbnb.Domain.DataTransferObjects.Booking;
using Mapster;

namespace Airbnb.Application.Mapester
{
    public class BookingMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<BookingToPaymentDto, BookingToPayment>();

            config.NewConfig<BookingToPayment, BookingToPaymentDto>();

            config.NewConfig<CreateCustomerBookingsCommand, CustomerBookings>()
                 .Map(dest => dest.Booking, src=>src.Booking);
        }
    }
}
