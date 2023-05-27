using ResellHub.Entities;
using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name can be up to 20 characters long")]
        [MinLength(4, ErrorMessage = "Name must be at least 4 characters long")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Phone number must consist of 9 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "City name can be up to 20 characters long")]
        [MinLength(3, ErrorMessage = "City name must be at least 3 characters long")]
        public string City { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "It is not correct form for e-mail")]
        public string Email { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Password can be up to 30 characters long")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string Password { get; set; }
    }
}
