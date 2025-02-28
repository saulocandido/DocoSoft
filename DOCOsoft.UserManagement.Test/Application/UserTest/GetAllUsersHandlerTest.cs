using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Application.Users.Dtos;
using DOCOsoft.UserManagement.Application.Users.Queries.GetAllUser;
using DOCOsoft.UserManagement.Domain.Entities;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using Moq;
using Xunit;

namespace DOCOsoft.UserManagement.Test.Application.UserTest
{
    public class GetAllUsersHandlerTests
    {
        private readonly Mock<IUserRepository> _mockUserRepo;
        private readonly GetAllUsersHandler _handler;

        public GetAllUsersHandlerTests()
        {
            _mockUserRepo = new Mock<IUserRepository>();
            _handler = new GetAllUsersHandler(_mockUserRepo.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUsersExist()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var roleId = Guid.NewGuid();

            var user = new User(new FullName("John", "Doe"), new Email("john.doe@example.com"));

            user.Id = userId;

            var role = new Role(roleId, "Admin");
            user.AddRole(role);

            var users = new List<User> { user };
            _mockUserRepo.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(users);

            var query = new GetAllUsersQuery();

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data.Users);

            var userDto = result.Data.Users.First();
            Assert.Equal("john.doe@example.com", userDto.Email);
            Assert.Equal("John", userDto.FirstName);
            Assert.Equal("Doe", userDto.LastName);
            Assert.Single(userDto.Roles);
            Assert.Equal("Admin", userDto.Roles.First().Name);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenNoUsersFound()
        {
            // Arrange
            _mockUserRepo.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(new List<User>());
            var query = new GetAllUsersQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            // Update expected message as needed.
            Assert.Equal("An error occurred", result.Message);
        }
    }
}
