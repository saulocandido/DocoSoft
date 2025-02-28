using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Domain.Events;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DOCOsoft.UserManagement.Application.Users.Commands.UpdateUser
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUserUniquenessChecker _uniquenessChecker;
        private readonly ILogger<UpdateUserHandler> _logger;

        public UpdateUserHandler(IUserRepository userRepo, IUserUniquenessChecker uniquenessChecker, ILogger<UpdateUserHandler> logger)
        {
            _userRepo = userRepo;
            _uniquenessChecker = uniquenessChecker;
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

            try
            {
                user.UpdateUser(
                    new FullName(request.FirstName, request.LastName),
                    new Email(request.Email),
                    _uniquenessChecker
                );

                _logger.LogInformation("Saving updated user data for UserId: {UserId}", request.Id);
                await _userRepo.UpdateAsync(user);

                var updatedDto = new UserDto(user.Id, user.Name.FirstName, user.Name.LastName, user.Email.Value);

                user.AddDomainEvent(new UserUpdatedEvent(user.Id, user.Name.FirstName, user.Name.LastName, user.Email.Value));

                _logger.LogInformation("User with ID {UserId} updated successfully.", request.Id);
                return Result<UserDto>.Success(updatedDto, "User updated successfully");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("User update failed: {Message}", ex.Message);
                return Result<UserDto>.Failure(ex.Message);
            }
        }
    }
}
