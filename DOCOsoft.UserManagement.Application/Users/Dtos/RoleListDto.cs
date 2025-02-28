using DOCOsoft.UserManagement.Application.Interfaces;
using System.Collections.Generic;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class RoleListDto : IDto
    {
        public List<RoleDto> Roles { get; }

        public RoleListDto(List<RoleDto> roles)
        {
            Roles = roles;
        }
    }
}
