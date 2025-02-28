using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;


namespace DOCOsoft.UserManagement.Application.Users.Queries.GetAllRoles
{
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, Result<RoleListDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result<RoleListDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync();

            if (roles is null || !roles.Any())
                return Result<RoleListDto>.Failure("No roles found.");

            var roleDtos = roles.Select(role => new RoleDto(role.Id, role.Name)).ToList();

            return Result<RoleListDto>.Success(new RoleListDto(roleDtos));
        }
    }
}
