using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.CachedObjects;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System.Net;
using System.Text.Json;

namespace Airbnb.Application.Features.PaymentBooking.Command.BookingCancelation
{
	public record BookingCancelationCommand : IRequest<Responses>
	{
		public string bookingId { get; set; }
		public BookingCancelationCommand(string bookingid)
		{
			bookingId = bookingid;
		}
	}
	public class BookingCancelationCommandHandler : IRequestHandler<BookingCancelationCommand, Responses>
	{
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly UserManager<AppUser> _userManager;
		private readonly IValidator<BookingCancelationCommand> _validator;
		private readonly IDistributedCache _cache;
		private readonly IUnitOfWork _unitOfWork;
		public BookingCancelationCommandHandler(
			IHttpContextAccessor contextAccessor,
			UserManager<AppUser> userManager,
			IValidator<BookingCancelationCommand> validator
,
			IDistributedCache cache,
			IUnitOfWork unitOfWork)
		{
			_contextAccessor = contextAccessor;
			_userManager = userManager;
			_validator = validator;
			_cache = cache;
			_unitOfWork = unitOfWork;
		}

		public async Task<Responses> Handle(BookingCancelationCommand request, CancellationToken cancellationToken)
		{
			var validation = await _validator.ValidateAsync(request);

			var jsonData = await _cache.GetStringAsync(request.bookingId);
			if (jsonData == null)
			{
				return await Responses.FailurResponse($"bookingItem with Id {request.bookingId} not found!", HttpStatusCode.NotFound);
			}
			var booking = JsonSerializer.Deserialize<CachedBooking>(jsonData!);


			var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
			if (user == null || booking.UserId != user.Id)
			{
				return await Responses.FailurResponse("UnAuthorized user!", HttpStatusCode.Unauthorized);
			}
			try
			{

				await _cache.RemoveAsync(request.bookingId);
				var notification=new Notification
				{
					Name = $"Booking with Id `{request.bookingId}` has been deleted!",
					UserId = user.Id,
					IsRead = false,
					CreatedAt = DateTime.Now
				};
				await _unitOfWork.Repository<Notification, int>().AddAsync(notification);
				await _unitOfWork.CompleteAsync();
				return await Responses.SuccessResponse("Booking has been deleted successfully.");
			}
			catch (Exception ex)
			{
				return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
			}

		}
	}

}
