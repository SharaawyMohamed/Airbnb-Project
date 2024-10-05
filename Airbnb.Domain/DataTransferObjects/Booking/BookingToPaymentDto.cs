using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Domain.DataTransferObjects.Booking
{
    public class BookingToPaymentDto
    {
        public int Id { get; set; }
        public string BookingType { get; set; }
        public string PropertyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
