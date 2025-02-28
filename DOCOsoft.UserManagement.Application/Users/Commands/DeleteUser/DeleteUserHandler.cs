using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DOCOsoft.UserManagement.Application.Users.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result<DeleteUserDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<DeleteUserHandler> _logger;

        public DeleteUserHandler(IUserRepository userRepo, ILogger<DeleteUserHandler> logger)
        {
            _userRepo = userRepo;
            _logger = logger;
        }

        public async Task<Result<DeleteUserDto>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteUserCommand for UserId: {UserId}", request.UserId);

            var user = await _userRepo.GetByIdAsync(request.UserId);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", request.UserId);
                return Result<DeleteUserDto>.Failure($"User with ID '{request.UserId}' not found.");
            }

            _logger.LogInformation("Deleting user with ID: {UserId}", request.UserId);
            await _userRepo.DeleteAsync(user);

            _logger.LogInformation("User with ID {UserId} deleted successfully.", request.UserId);
            return Result<DeleteUserDto>.Success(new DeleteUserDto(request.UserId), "User deleted successfully");
        }
    }
}
