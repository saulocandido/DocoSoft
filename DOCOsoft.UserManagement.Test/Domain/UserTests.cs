using DOCOsoft.UserManagement.Domain.Entities;
using DOCOsoft.UserManagement.Domain.Interfaces;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DOCOsoft.UserManagement.Test.Domain
{
    public class UserTests
    {
        private readonly Mock<IUserUniquenessChecker> _mockUniquenessChecker;
        private readonly UserDomainService _userDomainService;

        public UserTests()
        {
            _mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            _userDomainService = new UserDomainService(_mockUniquenessChecker.Object);
        }

        [Fact]
        public void CreateUser_ShouldCreateUser_WhenEmailIsUnique()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var roles = new List<Role> { new Role(Guid.NewGuid(), "Admin"), new Role(Guid.NewGuid(), "User") };
            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(true);

            // Act
            var user = _userDomainService.CreateUser(name, email, roles);

            // Assert
            Assert.Equal(name, user.Name);
            Assert.Equal(email, user.Email);
            Assert.Equal(2, user.Roles.Count);
        }

        [Fact]
        public void CreateUser_ShouldThrowException_WhenEmailIsNotUnique()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var roles = new List<Role>();
            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(false);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _userDomainService.CreateUser(name, email, roles));

            Assert.Equal("Email is already in use.", exception.Message);
        }

        [Fact]
        public void AddRole_ShouldAddRole_WhenRoleDoesNotExist()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var user = new User(name, email);
            var role = new Role(Guid.NewGuid(), "Admin");

            // Act
            user.AddRole(role);

            // Assert
            Assert.Contains(role, user.Roles);
        }

        [Fact]
        public void AddRole_ShouldNotAddDuplicateRole()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var roleId = Guid.NewGuid();
            var role = new Role(roleId, "Admin");
            var user = new User(name, email);
            user.AddRole(role); // Add role once

            // Act
            user.AddRole(role); // Try to add duplicate role

            // Assert
            Assert.Single(user.Roles); // Should still have only one role
        }

        [Fact]
        public void RemoveRole_ShouldRemoveRole_WhenRoleExists()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var roleId = Guid.NewGuid();
            var role = new Role(roleId, "Admin");
            var user = new User(name, email);
            user.AddRole(role);

            // Act
            user.RemoveRole(role);

            // Assert
            Assert.Empty(user.Roles);
        }

        [Fact]
        public void RemoveRole_ShouldDoNothing_WhenRoleDoesNotExist()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var user = new User(name, email);
            var role = new Role(Guid.NewGuid(), "Admin");

            // Act
            user.RemoveRole(role); // Removing non-existing role

            // Assert
            Assert.Empty(user.Roles); // Should remain empty
        }

        [Fact]
        public void UpdateUser_ShouldUpdateUser_WhenEmailIsUnique()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var user = new User(name, email);
            var newName = new FullName("Johnny", "Doe");
            var newEmail = new Email("johnny.doe@example.com");

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(true);

            // Act
            _userDomainService.UpdateUser(user, newName, newEmail);

            // Assert
            Assert.Equal(newName, user.Name);
            Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void UpdateUser_ShouldThrowException_WhenEmailIsNotUnique()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var user = new User(name, email);
            var newEmail = new Email("existing@example.com");
            var newName = new FullName("Johnny", "Doe");

            _mockUniquenessChecker.Setup(x => x.IsUserEmailUnique(It.IsAny<Email>())).Returns(false);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() =>
                _userDomainService.UpdateUser(user, newName, newEmail));

            Assert.Equal("Email is already in use.", exception.Message);
        }
    }
}
