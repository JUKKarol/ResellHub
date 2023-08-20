namespace ResellHub.DTOs.UserDTOs
{
    public class UserRespondListDto
    {
        public int PagesCount { get; set; }
        public int ItemsCount { get; set; }
        public List<UserPublicDto> Items { get; set; }
    }
}
