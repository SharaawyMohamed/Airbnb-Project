using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.CachedObjects;
using Airbnb.Domain.DataTransferObjects.User;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Migrations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.PaymentBooking.Command.PayBooking
{
	public record PayBookingCommand : IRequest<Responses>
	{
		public string BookingId { get; set; }
		public PayBookingCommand(string bookingId)
		{
			BookingId = bookingId;
		}
	}

	public class PayBookingCommandHandler : IRequestHandler<PayBookingCommand, Responses>
	{
		private readonly IConfiguration _config;
		private readonly IDistributedCache _cache;
		private readonly UserManager<AppUser> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IUnitOfWork _unitOfWork;
		public PayBookingCommandHandler(IConfiguration config, IDistributedCache cache, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor, IUnitOfWork unitOfWork)
		{
			_config = config;
			_cache = cache;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
			_unitOfWork = unitOfWork;
		}
		public async Task<Responses> Handle(PayBookingCommand request, CancellationToken cancellationToken)
		{
			StripeConfiguration.ApiKey = _config["Stripe:SecretKey"];

			var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);

			var jsonBooking = await _cache.GetStringAsync(request.BookingId);
			if (jsonBooking == null) return await Responses.FailurResponse($"In valid booking Id {request.BookingId} !", HttpStatusCode.NotFound);

			var booking = JsonConvert.DeserializeObject<CachedBooking>(jsonBooking!);
			if (user.Id != booking!.UserId)
			{
				return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);
			}

			// Stripe Logic

			var Service = new PaymentIntentService();
			var options = new PaymentIntentCreateOptions()
			{
				Amount = (long)(booking!.TotalPrice * 100),// we multiply in 100 because interact with money as percentage until faction not lost
				Currency = "usd",
				PaymentMethodTypes = new List<string>() { "card" },

			};
			var paymentIntent = await Service.CreateAsync(options);
			var paymentIntentBooking = new BookingPaymentDto()
			{
				BookingType = booking.BookingType,
				TotalPrice = booking.TotalPrice,
				StartDate = booking.StartDate,
				EndDate = booking.EndDate,
				PaymentMethod = "card",
				BookingDate = booking.BookingDate,
				PaymentStatus = "Pending",
				PropertyId = booking.PropertyId,
				UserId = user.Id,
				User = new OwnerDto
				{
					FullName = user.FullName,
					Email = user.Email!,
					PhoneNumber = user.PhoneNumber!
				},
				PaymentIntentId = paymentIntent.Id,
				ClientSecret = paymentIntent.ClientSecret,


			};
			var notification = new Notification()
			{
				UserId = user.Id,
				Name = $"Your booking with Id {request.BookingId} is successfully created!",
				IsRead = false,
				CreatedAt = DateTime.Now
			};
			await _unitOfWork.Repository<Notification, int>().AddAsync(notification);
			await _unitOfWork.CompleteAsync();

			return await Responses.SuccessResponse(paymentIntentBooking);

		}
	}
}
