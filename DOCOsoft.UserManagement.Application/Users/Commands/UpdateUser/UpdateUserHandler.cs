using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Domain.DomainServices;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DOCOsoft.UserManagement.Application.Users.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly UserDomainService _userDomainService;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUserRepository userRepo, UserDomainService userDomainService, ILogger<UpdateUserHandler> logger)
        {
            _userRepo = userRepo;
            _userDomainService = userDomainService;
            _logger = logger;
        }

        public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateUserCommand for UserId: {UserId}", request.Id);

            var user = await _userRepo.GetByIdAsync(request.Id);
            if (user is null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", request.Id);
                return Result<UserDto>.Failure($"User with ID '{request.Id}' not found.");
            }

            var fullName = new FullName(request.FirstName, request.LastName);
            var email = new Email(request.Email);

            // Delegate validation and update logic to the domain service
            _userDomainService.UpdateUser(user, fullName, email);

            _logger.LogInformation("Saving updated user data for UserId: {UserId}", request.Id);
            await _userRepo.UpdateAsync(user);

            var updatedDto = new UserDto(user.Id, user.Name.FirstName, user.Name.LastName, user.Email.Value);

            _logger.LogInformation("User with ID {UserId} updated successfully.", request.Id);
            return Result<UserDto>.Success(updatedDto, "User updated successfully");
        }
    }
}
