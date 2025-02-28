using DOCOsoft.UserManagement.Domain.Common;

namespace DOCOsoft.UserManagement.Domain.Events
{
    public sealed class UserUpdatedEvent : BaseDomainEvent
    {
        public Guid UserId { get; }
        public string Email { get; }
        public string FirstName { get; }
        public string LastName { get; }

        public UserUpdatedEvent(Guid userId, string email, string firstName, string lastName)
        {
            UserId = userId;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
