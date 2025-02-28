using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;


namespace DOCOsoft.UserManagement.Application.Users.Queries.GetAllUser
{
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, Result<UserListDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetAllUsersHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserListDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllUsersAsync();

            if (users is null || !users.Any())
                return Result<UserListDto>.Failure("No users found.");

            var userDtos = users.Select(user => new UserDto(
                user.Id,
                user.Name.FirstName,
                user.Name.LastName,
                user.Email.Value,
                user.Roles.Select(role => new RoleDto(role.Id, role.Name)).ToList()
            )).ToList();

            return Result<UserListDto>.Success(new UserListDto(userDtos));
        }
    }
}
