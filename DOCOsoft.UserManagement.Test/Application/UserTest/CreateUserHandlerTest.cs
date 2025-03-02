
using DOCOsoft.UserManagement.Application.Users.Commands.CreateUser;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

using DomainUser = DOCOsoft.UserManagement.Domain.Entities.User;
using DomainRole = DOCOsoft.UserManagement.Domain.Entities.Role;
using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Interfaces;

namespace DOCOsoft.UserManagement.Test.Application.UserTest
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRoleRepository> _mockRoleRepo;
        private readonly Mock<IUserUniquenessChecker> _mockUniquenessChecker;
        private readonly Mock<ILogger<CreateUserHandler>> _mockLogger;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRoleRepo = new Mock<IRoleRepository>();
            _mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            _mockLogger = new Mock<ILogger<CreateUserHandler>>();

            _handler = new CreateUserHandler(
                _mockUserRepo.Object,
                _mockRoleRepo.Object,
                _mockUniquenessChecker.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateUser_WhenEmailIsUnique()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var command = new CreateUserCommand(
                "John",
                "Doe",
                "test@example.com",
                new List<Guid> { roleId }
            );

            var email = new Email(command.Email);
            var fullName = new FullName(command.FirstName, command.LastName);

            var role = new DomainRole(roleId, "Admin");
            var roles = new List<DomainRole> { role };

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(email)).Returns(true);
            _mockRoleRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(roles);
            _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<DomainUser>())).Returns(Task.CompletedTask);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.Equal(command.Email, result.Data.Email);
            Assert.Equal(command.FirstName, result.Data.FirstName);
            Assert.Equal(command.LastName, result.Data.LastName);
            Assert.Single(result.Data.Roles);
            Assert.Equal("Admin", result.Data.Roles.First().Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEmailIsAlreadyInUse()
        {
            var command = new CreateUserCommand(
                "Jane",
                "Doe",
                "duplicate@example.com",
                new List<Guid> { Guid.NewGuid() }
            );

            var email = new Email(command.Email);

            _mockRoleRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<DomainRole>());
            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(email)).Returns(false);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Email is already in use.", result.Errors.FirstOrDefault());
            _mockUserRepo.Verify(x => x.AddAsync(It.IsAny<DomainUser>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenRepositoryThrowsException()
        {
            // Arrange
            var roleId = Guid.NewGuid();
            var command = new CreateUserCommand(
                "Bob",
                "Brown",
                "bob@example.com",
                new List<Guid> { roleId }
            );

            var email = new Email(command.Email);
            var fullName = new FullName(command.FirstName, command.LastName);
            var role = new DomainRole(roleId, "User");
            var roles = new List<DomainRole> { role };

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(email)).Returns(true);
            _mockRoleRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(roles);
            // Simulate an exception in AddAsync
            _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<DomainUser>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }

        [Fact]
        public void CreateUserCommand_InvalidEmail_ShouldThrowArgumentException()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var _ = new Email("invalidemail.com");
            });
        }
    }
}
