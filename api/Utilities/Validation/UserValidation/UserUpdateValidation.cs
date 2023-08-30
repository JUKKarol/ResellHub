using FluentValidation;
using ResellHub.DTOs.UserDTOs;
using System.Text.RegularExpressions;

namespace ResellHub.Utilities.Validation.UserValidation
{
    public class UserUpdateValidation : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidation()
        {
            RuleFor(u => u.PhoneNumber)
                .Must(phoneNumber => string.IsNullOrEmpty(phoneNumber) || Regex.IsMatch(phoneNumber, @"^[0-9]{9}$"))
                .WithMessage("Phone number must consist of 9 digits.");

            RuleFor(u => u.City)
                .Length(3, 20).WithMessage("City name must be between 3 and 20 characters long.")
                .When(u => !string.IsNullOrEmpty(u.City));

            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("Invalid email format.")
                .When(u => !string.IsNullOrEmpty(u.Email));
        }
    }
}