using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;
using System;

namespace DOCOsoft.UserManagement.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid UserId)
        : IRequest<Result<DeleteUserDto>>;
}
