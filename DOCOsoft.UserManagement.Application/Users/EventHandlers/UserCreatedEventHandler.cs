using DOCOsoft.UserManagement.Domain.Events;
using MediatR;


namespace DOCOsoft.UserManagement.Application.Users.EventHandlers
{
    public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
    {
        public UserCreatedEventHandler()
        {
        }

        public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
        {
            // example this would handle the UserCreatedEvent example email something 
            await Task.CompletedTask;
        }
    }
}
