using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Commands.CreateUser;
using DOCOsoft.UserManagement.Application.Users.Common;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Domain.Events;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUserRepository _userRepo;
    private readonly IRoleRepository _roleRepo;
    private readonly UserDomainService _userDomainService;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(
        IUserRepository userRepo,
        IRoleRepository roleRepo,
        UserDomainService userDomainService,
        ILogger<CreateUserHandler> logger)
    {
        _userRepo = userRepo;
        _roleRepo = roleRepo;
        _userDomainService = userDomainService;
        _logger = logger;
    }

    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateUserCommand for email: {Email}", request.Email);

        var email = new Email(request.Email);
        var fullName = new FullName(request.FirstName, request.LastName);

        _logger.LogInformation("Fetching roles for user.");

        var roles = (await _roleRepo.GetAllAsync())
            .Where(r => request.RoleIds.Contains(r.Id))
            .ToList();

        var user = _userDomainService.CreateUser(fullName, email, roles);

        _logger.LogInformation("Saving new user to repository.");
        await _userRepo.AddAsync(user);

        var userDto = new UserDto(user.Id, user.Name.FirstName, user.Name.LastName, user.Email.Value,
            roles.Select(r => new RoleDto(r.Id, r.Name)).ToList());

        user.AddDomainEvent(new UserCreatedEvent(user.Id, user.Email.Value));

        _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
        return Result<UserDto>.Success(userDto, "User created successfully");

    }
}
