using DOCOsoft.UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DOCOsoft.UserManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Explicitly configure `User` without seeding it
            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Email, email =>
                {
                    email.Property(e => e.Value)
                         .HasColumnName("Email")
                         .IsRequired()
                         .HasMaxLength(100);
                });

            modelBuilder.Entity<User>()
                .OwnsOne(u => u.Name, name =>
                {
                    name.Property(n => n.FirstName)
                        .HasColumnName("FirstName")
                        .IsRequired()
                        .HasMaxLength(50);

                    name.Property(n => n.LastName)
                        .HasColumnName("LastName")
                        .IsRequired()
                        .HasMaxLength(50);
                });

            // ✅ Configure Many-to-Many Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            // ✅ Seed Only Roles (Hardcoded IDs)
            var adminRoleId = new Guid("11111111-1111-1111-1111-111111111111");
            var userRoleId = new Guid("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<Role>().HasData(
                new Role(adminRoleId, "Admin"),
                new Role(userRoleId, "User")
            );
        }
    }
}
