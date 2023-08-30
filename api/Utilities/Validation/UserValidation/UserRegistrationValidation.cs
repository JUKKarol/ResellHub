using FluentValidation;
using ResellHub.DTOs.UserDTOs;

namespace ResellHub.Utilities.Validation.UserValidation
{
    public class UserRegistrationValidation : AbstractValidator<UserRegistrationDto>
    {
        public UserRegistrationValidation()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Name is required.")
                .Length(4, 20).WithMessage("Name must be between 4 and 20 characters long.");

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^[0-9]{9}$").WithMessage("Phone number must consist of 9 digits.");

            RuleFor(u => u.City)
                .NotEmpty().WithMessage("City name is required.")
                .Length(3, 20).WithMessage("City name must be between 3 and 20 characters long.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 30).WithMessage("Password must be between 8 and 30 characters long.");
        }
    }
}