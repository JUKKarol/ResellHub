using Microsoft.EntityFrameworkCore;
using ResellHub.Entities;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Offer>(eb =>
            {
                eb.HasKey(o => o.Id);
                eb.HasOne(o => o.User)
                    .WithMany(o => o.Offers)
                    .HasForeignKey(o => o.UserId);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.PhoneNumber).IsRequired();
                entity.Property(e => e.City).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.VeryficationToken).HasMaxLength(255);
                entity.Property(e => e.PasswordResetToken).HasMaxLength(255);
                entity.HasMany(e => e.Offers).WithOne(e => e.User);
                entity.HasMany(e => e.SentMessages).WithOne(e => e.FromUser);
                entity.HasMany(e => e.ReceivedMessages).WithOne(e => e.ToUser);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();

                entity.HasOne(e => e.FromUser)
                .WithMany(e => e.SentMessages)
                .HasForeignKey(e => e.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.ToUser)
                .WithMany(e => e.ReceivedMessages)
                .HasForeignKey(e => e.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder
                .UseSqlServer(_configuration.GetConnectionString("ResellHubConnectionString"));
        }
    }
}
