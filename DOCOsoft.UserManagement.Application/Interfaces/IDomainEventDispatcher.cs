using DOCOsoft.UserManagement.Domain.Interfaces;


namespace DOCOsoft.UserManagement.Application.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(IEnumerable<IDomainEvent> domainEvents);
    }
}
