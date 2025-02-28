using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;

namespace DOCOsoft.UserManagement.Application.Users.Queries.GetAllRoles
{
    public class GetAllRolesQuery : IRequest<Result<RoleListDto>>
    {
    }
}
