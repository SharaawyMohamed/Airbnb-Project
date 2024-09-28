using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Query
{
    public class GetUserBookingsQueryDto
    {
        public string PropertyId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public DateTimeOffset PaymentDate { get; set; }
    }
}
