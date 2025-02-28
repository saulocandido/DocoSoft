using DOCOsoft.UserManagement.Domain.Entities;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Domain.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOCOsoft.UserManagement.Test.Domain
{
    public class UserTests
    {
        [Fact]
        public void CreateUser_ShouldCreateUser_WhenEmailIsUnique()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var roles = new List<Role> { new Role(Guid.NewGuid(), "Admin"), new Role(Guid.NewGuid(), "User") };
            var mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            mockUniquenessChecker
                .Setup(x => x.IsUserEmailUnique(It.IsAny<Email>()))
                .Returns(true);

            // Act
            var user = User.CreateUser(name, email, mockUniquenessChecker.Object, roles);

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
            var mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            mockUniquenessChecker
                .Setup(x => x.IsUserEmailUnique(It.IsAny<Email>()))
                .Returns(false);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                User.CreateUser(name, email, mockUniquenessChecker.Object, roles));
        }

        [Fact]
        public void AddRole_ShouldAddRole_WhenRoleDoesNotExist()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            mockUniquenessChecker
                .Setup(x => x.IsUserEmailUnique(It.IsAny<Email>()))
                .Returns(true);
            var user = User.CreateUser(name, email, mockUniquenessChecker.Object, new List<Role>());
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
            var mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            mockUniquenessChecker
                .Setup(x => x.IsUserEmailUnique(It.IsAny<Email>()))
                .Returns(true);
            var user = User.CreateUser(name, email, mockUniquenessChecker.Object, new List<Role> { role });

            // Act
            user.AddRole(role);

            // Assert
            Assert.Single(user.Roles);
        }

        [Fact]
        public void RemoveRole_ShouldRemoveRole_WhenRoleExists()
        {
            // Arrange
            var name = new FullName("John", "Doe");
            var email = new Email("john.doe@example.com");
            var roleId = Guid.NewGuid();
            var role = new Role(roleId, "Admin");
            var mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            mockUniquenessChecker
                .Setup(x => x.IsUserEmailUnique(It.IsAny<Email>()))
                .Returns(true);
            var user = User.CreateUser(name, email, mockUniquenessChecker.Object, new List<Role> { role });

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
            var mockUniquenessChecker = new Mock<IUserUniquenessChecker>();
            mockUniquenessChecker
                .Setup(x => x.IsUserEmailUnique(It.IsAny<Email>()))
                .Returns(true);
            var user = User.CreateUser(name, email, mockUniquenessChecker.Object, new List<Role>());
            var role = new Role(Guid.NewGuid(), "Admin");

            // Act
            user.RemoveRole(role);

            // Assert
            Assert.Empty(user.Roles);
        }
    }
}
