using DOCOsoft.UserManagement.Application.Behaviors;
using DOCOsoft.UserManagement.Domain.DomainServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace DOCOsoft.UserManagement.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DomainEventsDispatcherBehavior<,>));
            services.AddScoped<UserDomainService>();

            return services;
        }
    }
}
