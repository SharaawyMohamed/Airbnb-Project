using Airbnb.Application.Utility;
using Airbnb.Domain;
using Airbnb.Domain.DataTransferObjects.Review;
using Airbnb.Domain.Entities;
using Airbnb.Domain.Identity;
using Airbnb.Domain.Interfaces.Repositories;
using Airbnb.Domain.Interfaces.Services;
using Airbnb.Infrastructure.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Airbnb.Application.Services
{
    public class ReviewServices : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ReviewServices(IUnitOfWork unitOfWork, IMapper mapper, IReviewRepository reviewRepository, UserManager<AppUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        public async Task<Responses> GetReviewById(int id)
        {

            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id);
            if (review == null)
            {
                return await Responses.FailurResponse($"Not found review with id {id}");
            }
            return await Responses.SuccessResponse(_mapper.Map<ReviewDto>(review));
        }
        public async Task<Responses> GetUserReviews(string userId)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null || user.Id != userId)
                return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);

            var spec = new ReviewWithSpec(userId: userId);
            var reviews = await _unitOfWork.Repository<Review, int>().GetAllWithSpecAsync(spec)!;

            if (reviews == null)
            {
                return await Responses.FailurResponse("No reviews found.");
            }
            return await Responses.SuccessResponse(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
        }
        public async Task<Responses> GetPropertyReviews(string? propertyId)
        {
            var spec = new ReviewWithSpec(propertyId: propertyId);
            var reviews = await _unitOfWork.Repository<Review, int>().GetAllWithSpecAsync(spec)!;

            if (reviews == null)
            {
                return await Responses.FailurResponse("No reviews found.");
            }
            return await Responses.SuccessResponse(_mapper.Map<IEnumerable<ReviewDto>>(reviews));
        }
        public async Task<Responses> AddReviewAsync(CreateReviewDto review)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null || user.Id != review.UserId) return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);

            var property = await _unitOfWork.Repository<Property, string>().GetByIdAsync(review.PropertyId)!;
            if (property == null)
            {
                return await Responses.FailurResponse($"Not found property with Id {review.PropertyId}!");
            }
            try
            {
                var Review = new Review
                {
                    Name = review.Comment,
                    UserId = user.Id,
                    PropertyId = property.Id,
                    Stars = review.Stars
                };
                await _unitOfWork.Repository<Review, int>().AddAsync(Review);
                await _unitOfWork.CompleteAsync();
                var countAndSum = await _reviewRepository.CountReviewsAdnSumStars(review.PropertyId);
                property.Rate = (countAndSum.Item2 / (float)countAndSum.Item1);

                _unitOfWork.Repository<Property, string>().Update(property);
                await _unitOfWork.CompleteAsync();
                return await Responses.SuccessResponse("Commend added successfully!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<Responses> DeleteReviewAsync(int id)
        {
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            var review = await _unitOfWork.Repository<Review, int>().GetByIdAsync(id)!;
            if (review == null)
            {
                return await Responses.FailurResponse($"InValid Id {id}!", HttpStatusCode.InternalServerError);
            }

            if (user == null || user.Id != review.UserId)
            {
                return await Responses.FailurResponse("UnAuthorized!", HttpStatusCode.Unauthorized);
            }

            try
            {
                _unitOfWork.Repository<Review, int>().Remove(review);
                await _unitOfWork.CompleteAsync();
                return await Responses.SuccessResponse($"Review has been deleted successfully!");
            }
            catch (Exception ex)
            {
                return await Responses.FailurResponse(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
        public async Task<Responses> UpdateReviewAsync(ReviewDto review)
        {
            var existingReview = await _unitOfWork.Repository<Review, int>().GetByIdAsync(review.ReviewId)!;
            if (existingReview == null)
            {
                return await Responses.FailurResponse($"Review with ID {review.ReviewId} not found.", HttpStatusCode.NotFound);
            }
            var user = await GetUser.GetCurrentUserAsync(_contextAccessor, _userManager);
            if (user == null || user.Id!=review.UserId)
            {
                return await Responses.FailurResponse($"UnAuthorized.", HttpStatusCode.Unauthorized);
            }
            existingReview.Stars = review.Stars;
            existingReview.Name = review.Comment;
             _unitOfWork.Repository<Review,int>().Update(existingReview);
            await _unitOfWork.CompleteAsync();
            return await Responses.SuccessResponse(review, "Review updated successfully.");
        }

	}

}