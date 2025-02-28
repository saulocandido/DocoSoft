using DOCOsoft.UserManagement.Domain.Common;


namespace DOCOsoft.UserManagement.Domain.Events
{
    public sealed class UserCreatedEvent : BaseDomainEvent
    {
        public Guid UserId { get; }
        public string Email { get; }

        public UserCreatedEvent(Guid userId, string email)
        {
            UserId = userId;
            Email = email;
        }
    }
}
