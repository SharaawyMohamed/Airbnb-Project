using Airbnb.Domain.Entities;
using Airbnb.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Airbnb.Domain;
using Airbnb.Infrastructure.Specifications;
using Airbnb.Domain.Interfaces.Repositories;
using AutoMapper;
using FluentValidation;
using Airbnb.APIs.Validators;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Airbnb.Domain.Identity;
using Airbnb.Domain.DataTransferObjects.Review;
using Microsoft.AspNetCore.Authorization;

namespace Airbnb.APIs.Controllers
{
    [Authorize(Roles ="Customer")]
    public class ReviewController : APIBaseController
    {
        private readonly IReviewService _reviewService;
        private readonly IValidator<ReviewDto> _validator;
        private readonly UserManager<AppUser> _userManager;
        public ReviewController(IReviewService reviewService,
            IValidator<ReviewDto> validator
,
            UserManager<AppUser> userManager)

        {
            _reviewService = reviewService;
            _validator = validator;
            _userManager = userManager;
        }

        [Authorize(Roles ="Owner")]
        [Authorize(Roles ="Admin")]
        [HttpGet("GetPropertyReviews")]
        public async Task<ActionResult<Responses>> GetPropertyReviews(string propertyId)
        {
            return Ok(await _reviewService.GetPropertyReviews(propertyId));
        }

        [HttpGet("GetUserReviews")]
        public async Task<ActionResult<Responses>> GetUserReviews(string userId)
        {
            return Ok(await _reviewService.GetUserReviews(userId));
        }

        [HttpGet("GetReviewById/{id}")]
        public async Task<ActionResult<Responses>> GetReview(int id)
        {
            return Ok(await _reviewService.GetReviewById(id));
        }

        [HttpDelete("DeleteReview/{id}")]
        public async Task<ActionResult<Responses>> DeleteReview(int id)
        {
            return Ok(await _reviewService.DeleteReviewAsync(id));
        }

        [HttpPost("CreateReview")]
        public async Task<ActionResult<Responses>> AddReview([FromBody] CreateReviewDto review)
        {
            return Ok(await _reviewService.AddReviewAsync(review));
        }

        [HttpPut("UpdateReview")]
        public async Task<ActionResult<Responses>> UpdateReview(ReviewDto review)
        {
            return Ok(await _reviewService.UpdateReviewAsync(review));
        }


    }
}
