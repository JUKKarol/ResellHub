using FluentValidation;
using ResellHub.DTOs.UserDTOs;

namespace ResellHub.Utilities.Validation.UserValidation
{
    public class UserResetPasswordValidation : AbstractValidator<UserResetPasswordDto>
    {
        public UserResetPasswordValidation()
        {
            RuleFor(u => u.Token)
                .NotEmpty().WithMessage("Token is required.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .Length(8, 30).WithMessage("Password must be between 8 and 30 characters long.");

            RuleFor(u => u.ConfirmPassword)
                .NotEmpty().WithMessage("Password is required.")
                .Equal(u => u.Password).WithMessage("Confirm password must be equal to password");
        }
    }
}