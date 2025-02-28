using DOCOsoft.UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace DOCOsoft.UserManagement.Infrastructure.Persistence.RoleRepository.TypeConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                   .ValueGeneratedOnAdd(); 

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(50);
        }
    }
}
