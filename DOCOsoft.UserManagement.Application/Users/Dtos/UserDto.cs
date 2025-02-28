using DOCOsoft.UserManagement.Application.Interfaces;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class UserDto : IDto
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public List<RoleDto> Roles { get; }

        private UserDto() { }

        public UserDto(Guid id, string firstName, string lastName, string email, List<RoleDto> roles)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Roles = roles;
        }

        public UserDto(Guid id, string firstName, string lastName, string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }
    }
}
