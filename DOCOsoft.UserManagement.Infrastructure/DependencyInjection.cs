using DOCOsoft.UserManagement.Application.Behaviors;
using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Infrastructure.Persistence;
using DOCOsoft.UserManagement.Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DOCOsoft.UserManagement.Infrastructure.Persistence.UserRepository;
using DOCOsoft.UserManagement.Infrastructure.Persistence.RoleRepository;
using DOCOsoft.UserManagement.Domain.Interfaces;


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

            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            // Domain Services
            services.AddScoped<IUserUniquenessChecker, UserUniquenessChecker>();

            return services;
        }

        public static void ApplyMigrations(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            using (var context = serviceProvider.GetService<AppDbContext>())
            {
                context.Database.Migrate();
            }
        }

        public static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("UserDatabaseConnectionString");

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
            ApplyMigrations(services);
        }

    }
}
