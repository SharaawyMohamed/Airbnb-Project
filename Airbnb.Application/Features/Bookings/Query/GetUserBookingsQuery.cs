using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Infrastructure.Specifications;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;
using booking = Airbnb.Domain.Entities.Booking;

namespace Airbnb.Application.Features.Bookings.Query
{
    public record GetUserBookingsQuery : IRequest<Responses>
    {
        public string userId { get; set; }
        public GetUserBookingsQuery(string userid)
        {
            userId = userid;
        }
    }
    public class GetUserBookingsQueryHandler : IRequestHandler<GetUserBookingsQuery, Responses>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserBookingsQueryHandler(IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Responses> Handle(GetUserBookingsQuery request, CancellationToken cancellationToken)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor,_userManager);
            if (user == null || user.Id != request.userId)
            {
                return await Responses.FailurResponse("UnAuthorized user!", HttpStatusCode.Unauthorized);
            }
            try
            {
                var spec = new BookingWithSpec(user.Id);
                var bookings = await _unitOfWork.Repository<booking, int>().GetAllWithSpecAsync(spec);
                var mapped = _mapper.Map<IEnumerable<GetUserBookingsQueryDto>>(bookings);
                return await Responses.SuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse($"Internal server error\n {ex.Message}!",HttpStatusCode.InternalServerError);
            }


        }
    }

}
