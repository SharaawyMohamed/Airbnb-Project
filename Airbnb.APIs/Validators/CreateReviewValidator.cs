using Airbnb.Domain.DataTransferObjects.Review;
using FluentValidation;

namespace Airbnb.APIs.Validators
{
	public class CreateReviewValidator:AbstractValidator<CreateReviewDto>
	{
        public CreateReviewValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.Comment).Length(3, 100);
            RuleFor(x => x.Stars).LessThan(6).GreaterThan(0);
        }
    }
}
