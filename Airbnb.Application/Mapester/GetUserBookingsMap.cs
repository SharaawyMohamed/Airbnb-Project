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
	public class GetUserBookingsMap : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Booking, GetUserBookingsDto>();

			config.NewConfig<AppUser, OwnerDto>();

			config.NewConfig<Property,PropertyUserDto>();	
		}
	}
}
