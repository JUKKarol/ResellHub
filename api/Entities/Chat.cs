namespace ResellHub.Entities
{
    public class Chat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
