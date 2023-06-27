namespace ResellHub.Entities
{
    public class Chat
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
        public DateTime LastMessageAt { get; set; } = DateTime.UtcNow;

        public virtual User Sender { get; set; }
        public virtual User Reciver { get; set; }
        public virtual List<Message> Messages { get; set; }
    }
}
