using System.Text.Json.Serialization;

namespace ResellHub.Entities
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FromUserId { get; set; }
        
        public Guid ToUserId { get; set; }
        
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public virtual User FromUser { get; set; }
        [JsonIgnore]
        public virtual User ToUser { get; set; }
    }
}
