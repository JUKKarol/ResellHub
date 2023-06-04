using ResellHub.Enums;

namespace ResellHub.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string EncodedName { get; set; }
        public string VeryficationToken { get; set; }
        public DateTime? VerifiedAt { get; set; } = null;
        public string PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; } = null;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public List<Role> Roles { get; set; }
        public List<Offer> Offers { get; set; }
        public List<Message> SentMessages { get; set; }
        public List<Message> ReceivedMessages { get; set; }
        public List<FollowOffer> FollowingOffers { get; set; }

        public void EncodeName() => EncodedName = Name.ToLower().Replace(" ", "-");
    }
}
