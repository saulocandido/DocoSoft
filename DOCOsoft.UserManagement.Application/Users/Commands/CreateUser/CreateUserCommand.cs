using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;
using System;
using System.Collections.Generic;

namespace DOCOsoft.UserManagement.Application.Users.Commands.CreateUser
{
    public record CreateUserCommand(string FirstName, string LastName, string Email, List<Guid> RoleIds)
        : IRequest<Result<UserDto>>;
}
