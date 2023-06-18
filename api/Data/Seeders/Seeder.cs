using Microsoft.AspNetCore.Identity;
using ResellHub.Entities;
using ResellHub.Services.OfferServices;
using ResellHub.Services.UserServices;
using ResellHub.Utilities.UserUtilities;

namespace ResellHub.Data.Seeders
{
    public class Seeder
    {
        private readonly ResellHubContext _context;
        private readonly IUserUtilities _userUtilities;

        public Seeder(ResellHubContext context, IUserUtilities userUtilities)
        {
            _context = context;
            _userUtilities = userUtilities;
        }

        public void Seed()
        {
            if (!_context.Users.Any() && !_context.Offers.Any())
            {
                string defaultPassword = "string";

                _userUtilities.CreatePasswordHash(defaultPassword,
                    out byte[] passwordHash,
                    out byte[] passwordSalt);

                User user1 = new User()
                {
                    Id = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    Name = "John Smith",
                    PhoneNumber = "324123567",
                    City = "New York City",
                    Email = "john.smith@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    VeryficationToken = _userUtilities.CreateRandomToken(),
                    VerifiedAt = DateTime.UtcNow,
                };
                user1.EncodeName();

                User user2 = new User()
                {
                    Id = Guid.Parse("87654321-4321-5678-9012-345678901234"),
                    Name = "Michael Johnson",
                    PhoneNumber = "539876543",
                    City = "Los Angeles",
                    Email = "michael.johnson@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    VeryficationToken = _userUtilities.CreateRandomToken(),
                    VerifiedAt = DateTime.UtcNow,
                };
                user2.EncodeName();

                User user3 = new User()
                {
                    Id = Guid.Parse("76543210-5678-1234-9012-345678901234"),
                    Name = "Robert Williams",
                    PhoneNumber = "789012343",
                    City = "Chicago",
                    Email = "robert.williams@example.com",
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    VeryficationToken = _userUtilities.CreateRandomToken(),
                    VerifiedAt = DateTime.UtcNow,
                };
                user3.EncodeName();

                _context.Users.AddRange(user1, user2, user3);
                _context.SaveChanges();

                Role role1 = new Role()
                {
                    UserId = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    UserRole = Enums.UserRoles.User
                };

                Role role2 = new Role()
                {
                    UserId = Guid.Parse("87654321-4321-5678-9012-345678901234"),
                    UserRole = Enums.UserRoles.Moderator
                };

                Role role3 = new Role()
                {
                    UserId = Guid.Parse("76543210-5678-1234-9012-345678901234"),
                    UserRole = Enums.UserRoles.Administrator
                };

                _context.Roles.AddRange(role1, role2, role3);
                _context.SaveChanges();

                Chat chat1 = new Chat()
                {
                    Id = Guid.Parse("23165432-1234-5678-9012-345678901234"),
                    FromUserId = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    ToUserId = Guid.Parse("87654321-4321-5678-9012-345678901234"),
                };

                _context.Chats.Add(chat1);
                _context.SaveChanges();

                Message message1 = new Message()
                {
                    FromUserId = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    ToUserId = Guid.Parse("87654321-4321-5678-9012-345678901234"),
                    ChatId = Guid.Parse("23165432-1234-5678-9012-345678901234"),
                    Content = "Is product still for sale?"
                };

                Message message2 = new Message()
                {
                    FromUserId = Guid.Parse("87654321-4321-5678-9012-345678901234"),
                    ToUserId = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    ChatId = Guid.Parse("23165432-1234-5678-9012-345678901234"),
                    Content = "No its not"
                };

                Message message3 = new Message()
                {
                    FromUserId = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    ToUserId = Guid.Parse("87654321-4321-5678-9012-345678901234"),
                    ChatId = Guid.Parse("23165432-1234-5678-9012-345678901234"),
                    Content = "Ok, thanks"
                };

                _context.Messages.AddRange(message1, message2, message3);
                _context.SaveChanges();

                Offer offer1 = new Offer()
                {
                    Id = Guid.Parse("33765432-1234-5678-9012-345678901234"),
                    Title = "Kaz Bałagane - Narkopop",
                    Brand = "Narkopop",
                    Category = "CD",
                    Description = "CD is still in foil, perfect condition",
                    Condition = 5,
                    PricePLN = 50,
                    ProductionYear = 2017,
                    UserId = Guid.Parse("98765432-1234-5678-9012-345678901234")
                };
                offer1.EncodeName();

                Offer offer2 = new Offer()
                {
                    Id = Guid.Parse("47765432-1234-5678-9012-345678901234"),
                    Title = "Ogrody hoodie",
                    Brand = "Ogrody",
                    Category = "Hoodie",
                    Description = "I was wear it few times, looks well",
                    Condition = 4,
                    PricePLN = 450,
                    ProductionYear = 2019,
                    UserId = Guid.Parse("98765432-1234-5678-9012-345678901234")
                };
                offer2.EncodeName();

                Offer offer3 = new Offer()
                {
                    Id = Guid.Parse("32765432-1234-5678-9012-345678901234"),
                    Title = "Pro8l3m playing cards",
                    Brand = "2020",
                    Category = "Accesories",
                    Description = "Often playing, damaged a little bit",
                    Condition = 2,
                    PricePLN = 60,
                    ProductionYear = 2022,
                    UserId = Guid.Parse("87654321-4321-5678-9012-345678901234")
                };
                offer3.EncodeName();

                _context.AddRange(offer1, offer2, offer3);
                _context.SaveChanges();

                FollowOffer followOffer1 = new FollowOffer()
                {
                    UserId = Guid.Parse("98765432-1234-5678-9012-345678901234"),
                    OfferId = Guid.Parse("32765432-1234-5678-9012-345678901234")
                };

                _context.Add(followOffer1);
                _context.SaveChanges();
            }
        }
    }
}
