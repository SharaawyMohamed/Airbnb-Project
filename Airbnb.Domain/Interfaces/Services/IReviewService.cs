using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Airbnb.Domain.DataTransferObjects.Review;
using Airbnb.Domain.Entities;

namespace Airbnb.Domain.Interfaces.Services
{
    public interface IReviewService
    {
        Task<Responses> GetReviewById(int id);
        Task<Responses> GetPropertyReviews(string propertyId);
        Task<Responses> GetUserReviews(string userId);
        Task<Responses> AddReviewAsync(CreateReviewDto review); 
        Task<Responses> UpdateReviewAsync(ReviewDto review); 
        Task<Responses> DeleteReviewAsync(int ReviewId);

    }
}
