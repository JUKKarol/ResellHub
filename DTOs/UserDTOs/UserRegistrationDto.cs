using ResellHub.Entities;

namespace ResellHub.DTOs.UserDTOs
{
    public class UserRegistrationDto
    {
        public string Name { get; set; }
        public int PhoneNumber { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string VeryficationToken { get; set; }
    }
}
