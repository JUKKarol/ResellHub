using System.Text.Json.Serialization;

namespace ResellHub.Entities
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReciverId { get; set; }
        
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual User Sender { get; set; }
        public virtual User Reciver { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
