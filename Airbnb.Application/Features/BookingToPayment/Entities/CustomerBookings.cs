using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.BookingToPayment.Entities
{
    public class CustomerBookings
    {
        public string Id { get; set; }
        public List<BookingToPayment> Bookings { get; set; }
        public CustomerBookings(string id)
        {
            Id = id;
        }

    }
}
