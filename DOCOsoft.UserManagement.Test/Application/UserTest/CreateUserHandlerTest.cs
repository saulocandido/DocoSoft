using DOCOsoft.UserManagement.Application.Users.Commands.CreateUser;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Domain.Entities;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using DOCOsoft.UserManagement.Domain.Interfaces;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DOCOsoft.UserManagement.Domain.Common;

namespace DOCOsoft.UserManagement.Test.Application.UserTest
{
    public class CreateUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<IRoleRepository> _mockRoleRepo;
        private readonly Mock<IUserUniquenessChecker> _mockUniquenessChecker;
        private readonly Mock<ILogger<CreateUserHandler>> _mockLogger;
        private readonly UserDomainService _userDomainService;
        private readonly CreateUserHandler _handler;

        public CreateUserHandlerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockRoleRepo = new Mock<IRoleRepository>();
            _mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            _mockLogger = new Mock<ILogger<CreateUserHandler>>();

            // Initialize UserDomainService with the mock uniqueness checker
            _userDomainService = new UserDomainService(_mockUniquenessChecker.Object);

            _handler = new CreateUserHandler(
                _mockUserRepo.Object,
                _mockRoleRepo.Object,
                _userDomainService,
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
            var role = new Role(roleId, "Admin");
            var roles = new List<Role> { role };

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(true);
            _mockRoleRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(roles);
            _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
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
            // Arrange
            var command = new CreateUserCommand(
                "Jane",
                "Doe",
                "duplicate@example.com",
                new List<Guid> { Guid.NewGuid() }
            );

            var email = new Email(command.Email);

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(false);
            _mockRoleRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(new List<Role>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));

            Assert.Equal("Email is already in use.", exception.Message);
        }


        [Fact]
        public async Task Handle_ShouldThrowException_WhenRepositoryThrowsException()
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
            var role = new Role(roleId, "User");
            var roles = new List<Role> { role };

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(true);
            _mockRoleRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(roles);
            _mockUserRepo.Setup(x => x.AddAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            Assert.Equal("Database error", ex.Message);
        }

        [Fact]
        public void CreateUserCommand_InvalidEmail_ShouldThrowDomainValidationException()
        {
            // Act & Assert
            var exception = Assert.Throws<DomainValidationException>(() =>
            {
                var _ = new Email("invalidemail.com"); // Missing '@' should trigger validation
            });

            Assert.Equal("Email must contain '@' symbol.", exception.Message);
        }

    }
}
