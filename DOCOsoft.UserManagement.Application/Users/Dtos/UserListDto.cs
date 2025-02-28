using DOCOsoft.UserManagement.Application.Interfaces;
using System.Collections.Generic;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class UserListDto : IDto
    {
        public List<UserDto> Users { get; }

        public UserListDto(List<UserDto> users)
        {
            Users = users;
        }
    }
}
