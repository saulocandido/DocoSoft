using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;

namespace DOCOsoft.UserManagement.Application.Users.Queries.GetAllUser
{
    public class GetAllUsersQuery : IRequest<Result<UserListDto>>
    {
    }
}
