using DOCOsoft.UserManagement.Application.Interfaces;
using System;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class UpdateUserDto : IDto
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }

        public UpdateUserDto(Guid userId, string firstName, string lastName, string email)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
