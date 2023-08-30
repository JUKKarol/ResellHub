using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserLoginDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "it is not correct form for e-mail")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}