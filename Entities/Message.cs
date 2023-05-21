using System.Text.Json.Serialization;

namespace ResellHub.Entities
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid FromUserId { get; set; }
        [JsonIgnore]
        public User FromUser { get; set; }
        public Guid ToUserId { get; set; }
        [JsonIgnore]
        public User ToUser { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
