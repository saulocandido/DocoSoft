using DOCOsoft.UserManagement.Domain.Common;
using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DOCOsoft.UserManagement.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options, IDomainEventDispatcher domainEventDispatcher)
            : base(options)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicit configuration for User
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

            // Configuring Many-to-Many Relationship with Role
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            // Seed Only Roles (Hardcoded IDs for demo)
            var adminRoleId = new Guid("11111111-1111-1111-1111-111111111111");
            var userRoleId = new Guid("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<Role>().HasData(
                new Role(adminRoleId, "Admin"),
                new Role(userRoleId, "User")
            );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            foreach (var entity in domainEntities)
            {
                await _domainEventDispatcher.DispatchEventsAsync(entity.DomainEvents);
                entity.ClearDomainEvents();
            }

            return result;
        }
    }
}
