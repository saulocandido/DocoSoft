using DOCOsoft.UserManagement.Domain.Entities;
using DOCOsoft.UserManagement.Domain.Interfaces;
using DOCOsoft.UserManagement.Domain.ValueObjects;

namespace DOCOsoft.UserManagement.Domain.Services
{
    /// <summary>
    /// a domain service for user domain 
    /// </summary>
    public class UserDomainService
    {
        private readonly IUserUniquenessChecker _uniquenessChecker;

        public UserDomainService(IUserUniquenessChecker uniquenessChecker)
        {
            _uniquenessChecker = uniquenessChecker;
        }

        public User CreateUser(FullName name, Email email, IEnumerable<Role> roles)
        {
            if (!_uniquenessChecker.IsUserEmailUnique(email))
                throw new InvalidOperationException("Email is already in use.");

            var user = new User(name, email);
            foreach (var role in roles)
            {
                user.AddRole(role);
            }

            return user;
        }

        public void UpdateUser(User user, FullName name, Email newEmail)
        {
            if (!user.Email.Equals(newEmail) && !_uniquenessChecker.IsUserEmailUnique(newEmail))
                throw new InvalidOperationException("Email is already in use.");

            user.UpdateUser(name, newEmail);
        }
    }
}
