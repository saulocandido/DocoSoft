using DOCOsoft.UserManagement.Application.Behaviors;
using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Interfaces;
using DOCOsoft.UserManagement.Infrastructure.Persistence;
using DOCOsoft.UserManagement.Infrastructure.Persistence.RoleRepository;
using DOCOsoft.UserManagement.Infrastructure.Persistence.UserRepository;
using DOCOsoft.UserManagement.Infrastructure.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DOCOsoft.UserManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            ConfigureDatabase(services, configuration);

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IUserUniquenessCheckerRepo, UserUniquenessCheckerRepo>();

            // Register the DomainEventDispatcher
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            // Domain Services
            services.AddScoped<IUserUniquenessChecker, UserUniquenessChecker>();

            return services;
        }

        public static void ApplyMigrations(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            using (var context = serviceProvider.GetRequiredService<AppDbContext>())
            {
                context.Database.Migrate();
            }
        }

        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            // Use the overload to get the service provider so that DI resolves all dependencies.
            services.AddDbContext<AppDbContext>((serviceProvider, options) =>
            {
                var connectionString = configuration.GetConnectionString("UserDatabaseConnectionString");
                options.UseSqlite(connectionString);
            });

            ApplyMigrations(services);
        }
    }
}
