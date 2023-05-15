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
        public DbSet<Role> Roles { get; set; }

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
                entity.HasMany(u=> u.Offers).WithOne(e => e.User);
                entity.HasMany(u=> u.SentMessages).WithOne(e => e.FromUser);
                entity.HasMany(u=> u.ReceivedMessages).WithOne(e => e.ToUser);
            });

            modelBuilder.Entity<Offer>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.HasOne(o => o.User)
                    .WithMany(u => u.Offers)
                    .HasForeignKey(o => o.UserId);
                entity.Property(o => o.Title).IsRequired().HasMaxLength(40);
                entity.Property(o => o.Description).HasMaxLength(200);
                entity.Property(o => o.ProductionYear).HasAnnotation("Range", new[] { 1950, DateTime.Now.Year });
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Content).IsRequired();

                entity.HasOne(m => m.FromUser)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.FromUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(m => m.ToUser)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ToUserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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
