using Airbnb.Application.Features.Bookings.Query.GetBookingById;
using Airbnb.Application.Features.Bookings.Query.GetUserBookings;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Mapester
{
	public class GetBookingByIdMap : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Booking, GetBookingByIdDto>();

			config.NewConfig<AppUser, OwnerDto>();

			config.NewConfig<Property, PropertyUserDto>();
		}
	}
}
