using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Password can be up to 30 characters long")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
