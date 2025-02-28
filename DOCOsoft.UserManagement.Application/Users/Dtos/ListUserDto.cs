using DOCOsoft.UserManagement.Application.Interfaces;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class ListUserDto :IDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
