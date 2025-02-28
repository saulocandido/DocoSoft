
namespace DOCOsoft.UserManagement.Domain.Common
{
    public abstract class BaseDomainEvent : IDomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
