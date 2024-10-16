using Airbnb.Domain.DataTransferObjects.Review;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
	public class UpdateReviewValidator:AbstractValidator<ReviewDto>
	{
        public UpdateReviewValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.Comment).Length(3, 100);
            RuleFor(x => x.Stars).LessThan(6).GreaterThan(0);
        }
    }
}
