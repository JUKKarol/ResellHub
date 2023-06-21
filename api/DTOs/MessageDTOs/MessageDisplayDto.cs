namespace ResellHub.DTOs.MessageDTOs
{
    public class MessageDisplayDto
    {
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }

        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
