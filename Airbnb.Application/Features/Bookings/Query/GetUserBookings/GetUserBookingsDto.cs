using Airbnb.Domain.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Query.GetUserBookings
{
	public class GetUserBookingsDto
	{
		public OwnerDto User { get; set; }
		public PropertyUserDto Property { get; set; }
		public decimal TotalPrice { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }
		public string PaymentMethod { get; set; } = string.Empty;
		public DateTimeOffset BookingDate { get; set; } = DateTimeOffset.Now;
		public DateTimeOffset PaymentDate { get; set; }
		public string PaymentStatus { get; set; } = string.Empty;
	}
}
