using DOCOsoft.UserManagement.Domain.Common;


namespace DOCOsoft.UserManagement.Application.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(IEnumerable<IDomainEvent> domainEvents);
    }
}
