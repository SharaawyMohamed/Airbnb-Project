using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Airbnb.Application.Features.Bookings.Command.BookingCancelation
{
    public record BookingCancelationCommand : IRequest<Responses>
    {
        public int bookingId { get; set; }
        public BookingCancelationCommand(int bookingid)
        {
            bookingId = bookingid;
        }
    }
    public class BookingCancelationCommandHandler : IRequestHandler<BookingCancelationCommand, Responses>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IValidator<BookingCancelationCommand> _validator;
        public BookingCancelationCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IValidator<BookingCancelationCommand> validator)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<Responses> Handle(BookingCancelationCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);

            var booking = await _unitOfWork.Repository<Booking, int>().GetByIdAsync(request.bookingId);
            if (booking == null)
            {
                return await Responses.FailurResponse($"Booking with Id {request.bookingId} not found!", HttpStatusCode.NotFound);

            }

            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null || booking.UserId != user.Id)
            {
                return await Responses.FailurResponse("UnAuthorized user!", HttpStatusCode.Unauthorized);
            }

            _unitOfWork.Repository<Booking, int>().Remove(booking);
            await _unitOfWork.CompleteAsync();
            return await Responses.SuccessResponse("Booking has been cancled successfully!");
        }
    }

}
