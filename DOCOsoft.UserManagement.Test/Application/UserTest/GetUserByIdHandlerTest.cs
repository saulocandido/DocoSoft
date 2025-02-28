using System;
using System.Threading;
using System.Threading.Tasks;
using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Commands.DeleteUser;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DOCOsoft.UserManagement.Test.Application.UserTest
{
    public class DeleteUserHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly Mock<ILogger<DeleteUserHandler>> _mockLogger;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _mockLogger = new Mock<ILogger<DeleteUserHandler>>();
            _handler = new DeleteUserHandler(_mockUserRepo.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand (userId);

            var user = new User(new DOCOsoft.UserManagement.Domain.ValueObjects.FullName("John", "Doe"),
                                new DOCOsoft.UserManagement.Domain.ValueObjects.Email("john.doe@example.com"));

            _mockUserRepo.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);
            _mockUserRepo.Setup(x => x.DeleteAsync(user)).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(userId, result.Data.UserId);
            Assert.Equal("User deleted successfully", result.Message);

            _mockUserRepo.Verify(x => x.DeleteAsync(user), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand(userId);

            _mockUserRepo.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal($"User with ID '{userId}' not found.", result.Errors.FirstOrDefault());

            _mockUserRepo.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
