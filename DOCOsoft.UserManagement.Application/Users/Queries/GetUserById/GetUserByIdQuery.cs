using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;
using System;

namespace DOCOsoft.UserManagement.Application.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid UserId)
        : IRequest<Result<UserDto>>;
}
