namespace ResellHub.DTOs.UserDTOs
{
    public class UserRespondListDto
    {
        public int CurrentPage { get; set; }
        public int PagesCount { get; set; }
        public int UsersCount { get; set; }
        public List<UserPublicDto> Users { get; set; }
    }
}
