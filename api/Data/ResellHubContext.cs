using Microsoft.EntityFrameworkCore;
using ResellHub.Entities;
using System.Linq;

namespace ResellHub.Data
{
    public class ResellHubContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ResellHubContext(DbContextOptions<ResellHubContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<FollowOffer> FollowingOffers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<AvatarImage> AvatarImages { get; set; }
        public DbSet<OfferImage> OfferImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired();
                entity.Property(u => u.PhoneNumber).IsRequired();
                entity.Property(u => u.City).IsRequired();
                entity.Property(u => u.Email).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.PasswordSalt).IsRequired();
                entity.Property(u => u.Slug).IsRequired();

                entity.HasMany(u => u.FromChats).WithOne(c => c.Sender);
                entity.HasMany(u => u.ToChats).WithOne(c => c.Reciver);
                entity.HasMany(u => u.Offers).WithOne(o => o.User);
                entity.HasMany(u => u.SentMessages).WithOne(m => m.Sender);
                entity.HasMany(u => u.ReceivedMessages).WithOne(m => m.Reciver);
                entity.HasMany(u => u.FollowingOffers).WithOne(m => m.User);
                entity.HasOne(u => u.AvatarImage)
                    .WithOne(ai => ai.User)
                    .HasForeignKey<AvatarImage>(ai => ai.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Offers)
                    .HasForeignKey(o => o.UserId);
                entity.HasOne(o => o.Category)
                    .WithMany(c => c.Offers)
                    .HasForeignKey(o => o.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(o => o.OfferImages)
                    .WithOne(oi => oi.Offer)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(o => o.Title).IsRequired().HasMaxLength(40);
                entity.Property(o => o.Description).HasMaxLength(200);
                entity.Property(o => o.ProductionYear).HasAnnotation("Range", new[] { 1950, DateTime.UtcNow.Year });
                entity.Property(o => o.Slug).IsRequired();
                entity.HasMany(u => u.FollowingOffers).WithOne(m => m.Offer);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Content).IsRequired();

                entity.HasOne(m => m.Chat)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(m => m.ChatId);

                entity.HasOne(m => m.Sender)
                    .WithMany(u => u.SentMessages)
                    .HasForeignKey(m => m.SenderId);

                entity.HasOne(m => m.Reciver)
                    .WithMany(u => u.ReceivedMessages)
                    .HasForeignKey(m => m.ReciverId);
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasOne(c => c.Sender)
                    .WithMany(u => u.FromChats)
                    .HasForeignKey(c => c.SenderId);

                entity.HasOne(c => c.Reciver)
                    .WithMany(u => u.ToChats)
                    .HasForeignKey(c => c.ReciverId);

                entity.HasMany(c => c.Messages)
                    .WithOne(m => m.Chat)
                    .HasForeignKey(m => m.ChatId);
            });

            modelBuilder.Entity<FollowOffer>(entity =>
            {
                entity.HasKey(fo => fo.Id);

                entity.HasOne(fo => fo.User)
                    .WithMany(u => u.FollowingOffers)
                    .HasForeignKey(fo => fo.UserId)
                    .OnDelete(deleteBehavior: DeleteBehavior.ClientCascade);

                entity.HasOne(fo => fo.Offer)
                    .WithMany(o => o.FollowingOffers)
                    .HasForeignKey(fo => fo.OfferId)
                    .OnDelete(deleteBehavior: DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.HasOne(r => r.RoleOwner)
                    .WithMany(u => u.Roles)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.UserId).IsRequired();
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.HasMany(c => c.Offers)
                    .WithOne(o => o.Category)
                    .HasForeignKey(o => o.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<AvatarImage>(entity =>
            {
                entity.HasKey(ai => ai.Id);

                entity.HasOne(ai => ai.User)
                    .WithOne(u => u.AvatarImage)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(ai => ai.ImageSlug).IsRequired();
                entity.Property(ai => ai.UserId).IsRequired();
            });

            modelBuilder.Entity<OfferImage>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.HasOne(oi => oi.Offer)
                    .WithMany(o => o.OfferImages)
                    .HasForeignKey(oi => oi.OfferId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.Property(oi => oi.ImageSlug).IsRequired();
                entity.Property(oi => oi.OfferId).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseNpgsql(_configuration.GetConnectionString("ResellHubConnectionString"));

            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
