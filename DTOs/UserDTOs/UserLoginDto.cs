using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserLoginDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
