using System.Text.Json.Serialization;

namespace ResellHub.Entities
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid ChatId { get; set; }
        public Guid FromUserId { get; set; }
        public Guid ToUserId { get; set; }
        
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
