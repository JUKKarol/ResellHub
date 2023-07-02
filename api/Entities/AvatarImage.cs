namespace ResellHub.Entities
{
    public class AvatarImage
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ImageSlug { get; set; }
        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
