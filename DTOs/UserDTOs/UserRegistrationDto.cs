using ResellHub.Entities;
using System.ComponentModel.DataAnnotations;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserRegistrationDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(20, ErrorMessage = "Name can have a maximum of 20 characters.")]
        [MinLength(4, ErrorMessage = "Name must have a minimum of 4 characters.")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[0-9]{9}$", ErrorMessage = "Phone number must consist of 9 digits")]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "City name can be maximum 20 characters")]
        public string City { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "it is not correct form for e-mail")]
        public string Email { get; set; }
        [Required]
        [StringLength(8, ErrorMessage = "Password must be at least 8 characters long", MinimumLength = 8)]
        public string Password { get; set; }
    }
}
