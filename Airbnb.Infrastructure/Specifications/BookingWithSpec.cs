

using Airbnb.Domain.Entities;

namespace Airbnb.Infrastructure.Specifications
{
    public class BookingWithSpec:BaseSpecifications<Booking,int>
    {
        public BookingWithSpec(string userId):
            base(B=>B.UserId==userId)
        {
            AddOrderByDescending(x => x.StartDate);
        }
    }
}
