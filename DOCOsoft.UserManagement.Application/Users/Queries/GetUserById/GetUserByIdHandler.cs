using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;


namespace DOCOsoft.UserManagement.Application.Users.Queries.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IRoleRepository _roleRepo;

        public GetUserByIdHandler(IUserRepository userRepo, IRoleRepository roleRepo)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepo.GetUserByIdAsync(request.UserId);
            if (user is null)
                return Result<UserDto>.Failure($"User with ID '{request.UserId}' not found.");

            var dto = new UserDto(
                user.Id,
                user.Name.FirstName,
                user.Name.LastName,
                user.Email.Value,
                user.Roles
                    .Select(role => new RoleDto(role.Id, role.Name))
                    .ToList()  
            );

            return Result<UserDto>.Success(dto, "User retrieved successfully");
        }
    }
}
