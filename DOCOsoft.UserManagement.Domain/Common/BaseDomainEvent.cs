
using DOCOsoft.UserManagement.Domain.Interfaces;
using MediatR;

namespace DOCOsoft.UserManagement.Domain.Common
{
    public abstract class BaseDomainEvent : INotification, IDomainEvent
    {
        public DateTime OccurredOn { get; protected set; } = DateTime.UtcNow;
    }
}
