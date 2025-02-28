using DOCOsoft.UserManagement.Application.Interfaces;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class DeleteUserDto : IDto
    {
        public Guid UserId { get; }

        public DeleteUserDto(Guid userId)
        {
            UserId = userId;
        }
    }
}
