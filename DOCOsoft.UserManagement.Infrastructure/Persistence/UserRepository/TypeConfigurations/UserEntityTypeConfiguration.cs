using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DOCOsoft.UserManagement.Domain.Entities;

namespace DOCOsoft.UserManagement.Infrastructure.Persistence.UserRepository.TypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd(); 

            builder.OwnsOne(u => u.Name, name =>
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

            builder.OwnsOne(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                     .HasColumnName("Email")
                     .IsRequired()
                     .HasMaxLength(100);
            });

            builder.HasMany(u => u.Roles)
                   .WithMany(r => r.Users)
                   .UsingEntity<Dictionary<string, object>>(
                        "UserRoles",
                        j => j.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                        j => j.HasOne<User>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");
                            j.ToTable("UserRoles"); 
                        }
                    );
        }
    }
}
