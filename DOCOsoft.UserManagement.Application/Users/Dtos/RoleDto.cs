using DOCOsoft.UserManagement.Application.Interfaces;
using System;

namespace DOCOsoft.UserManagement.Application.Users.Dtos
{
    public class RoleDto : IDto
    {
        public Guid Id { get; }
        public string Name { get; }

        public RoleDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
