using Airbnb.Domain;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using booking = Airbnb.Domain.Entities.Booking;

namespace Airbnb.Application.Features.Bookings.Command
{
    public record CreateBookingCommand : IRequest<Responses>
    {
        public string BookingType { get; set; }
        public string PropertyId { get; set; }
        public string UserId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
    internal class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, Responses>
    {
        private readonly IValidator<CreateBookingCommand> _validator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CreateBookingCommandHandler(IValidator<CreateBookingCommand> validator, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _validator = validator;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Responses> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(request);
            if (!validation.IsValid)
            {
                return await Responses.FailurResponse(validation.Errors.ToList());
            }

            var user = await GetCurrentUserAsync();
            if (user == null || user.Id != request.UserId)
            {
                return await Responses.FailurResponse("UnAuthorized user!", HttpStatusCode.Unauthorized);
            }

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(request.PropertyId);
            if (property == null)
            {
                return await Responses.FailurResponse($"Not found property with Id {request.PropertyId} !", HttpStatusCode.NotFound);
            }
            bool Isbooked = property.Bookings.Any(x =>
            x.StartDate >= request.StartDate && x.StartDate <= request.EndDate
            || x.EndDate >= request.StartDate && x.EndDate <= request.EndDate);

            if (Isbooked)
            {
                return await Responses.FailurResponse("this property already booked in this date range!");
            }
            var period = request.EndDate - request.StartDate;

            var mapped = _mapper.Map<booking>(request);

            // we need to create payment integration here
            mapped.TotalPrice = property.NightPrice * period.Days;
            mapped.PaymentDate = DateTimeOffset.Now;// default until paymend done

            try
            {
                await _unitOfWork.Repository<booking, int>().AddAsync(mapped);
                await _unitOfWork.CompleteAsync();
                return await Responses.SuccessResponse("this property already booked in this date range!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
            }

        }
        public async Task<AppUser>? GetCurrentUserAsync()
        {
            var userClaims = _contextAccessor.HttpContext?.User;

            if (userClaims == null || !userClaims.Identity.IsAuthenticated)
            {
                return null;
            }

            var userEmail = userClaims.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(userEmail))
            {
                return null;
            }

            return await _userManager.FindByEmailAsync(userEmail);

        }
    }
}
