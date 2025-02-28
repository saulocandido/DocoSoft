﻿using DOCOsoft.UserManagement.Domain.Common;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Domain.ValueObjects;

namespace DOCOsoft.UserManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        private List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        public FullName Name { get; private set; }
        public Email Email { get; private set; }
        public ICollection<Role> Roles { get; private set; } = new List<Role>();

        private User() { }

        public User(FullName name, Email email)
        {
            Name = name;
            Email = email;
        }

        public static User CreateUser(FullName name, Email email, IUserUniquenessChecker uniquenessChecker, IEnumerable<Role> roles)
        {
            if (!uniquenessChecker.IsUserEmailUnique(email))
                throw new InvalidOperationException("Email is already in use.");

            var user = new User(name, email);
            foreach (var role in roles)
            {
                user.AddRole(role);
            }
            return user;
        }

        public void UpdateUser(FullName name, Email email, IUserUniquenessChecker uniquenessChecker)
        {
            if (!Email.Equals(email) && !uniquenessChecker.IsUserEmailUnique(email))
                throw new InvalidOperationException("Email is already in use.");

            Name = name;
            Email = email;
            UpdateModifiedDate();
        }

        public void AddRole(Role role)
        {
            if (role != null && !Roles.Any(r => r.Id == role.Id))
                Roles.Add(role);
        }

        public void RemoveRole(Role role)
        {
            if (role != null && Roles.Any(r => r.Id == role.Id))
                Roles.Remove(role);
        }
    }
}
