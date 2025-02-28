using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Common;
using MediatR;



namespace DOCOsoft.UserManagement.Application.Behaviors
{
    public class DomainEventsDispatcherBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IDomainEventDispatcher _domainEventDispatcher;

        public DomainEventsDispatcherBehavior(IDomainEventDispatcher domainEventDispatcher)
        {
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            if (request is BaseEntity entity && entity.DomainEvents.Any())
            {
                var events = entity.DomainEvents.ToArray();
                entity.ClearDomainEvents();
                await _domainEventDispatcher.DispatchEventsAsync(events);
            }

            return response;
        }
    }
}
