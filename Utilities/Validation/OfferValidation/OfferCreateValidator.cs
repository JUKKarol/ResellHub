using FluentValidation;
using ResellHub.DTOs.OfferDTOs;

namespace ResellHub.Utilities.Validation.Offer
{
    public class OfferCreateValidator : AbstractValidator<OfferCreateDto>
    {
        public OfferCreateValidator()
        {
            RuleFor(o => o.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .Length(4, 30)
                .WithMessage("Title must be between 4 and 30 characters long.");

            RuleFor(o => o.Brand)
                .NotEmpty()
                .WithMessage("Brand is required.")
                .Length(2, 20)
                .WithMessage("Brand must be between 2 and 20 characters long.");

            RuleFor(o => o.Description)
                .MaximumLength(200)
                .WithMessage("Description can be up to 200 characters long.");

            RuleFor(o => o.Condition)
                .InclusiveBetween(1, 5)
                .WithMessage("Condition must be between 1 and 5.");

            RuleFor(o => o.PricePLN)
                .InclusiveBetween(1, 10000)
                .WithMessage("Price must be between 1 and 10,000.");

            RuleFor(o => o.ProductionYear)
                .InclusiveBetween(1950, DateTime.Now.Year + 1)
                .WithMessage("Incorrect year.");
        }
    }
}
