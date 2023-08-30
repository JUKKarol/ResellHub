namespace ResellHub.DTOs.UserDTOs
{
    public class UserResetPasswordDto
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}