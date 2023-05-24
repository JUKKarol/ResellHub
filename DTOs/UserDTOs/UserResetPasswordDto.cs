using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
