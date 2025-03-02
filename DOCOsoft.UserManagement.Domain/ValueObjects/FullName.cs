using DOCOsoft.UserManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOCOsoft.UserManagement.Domain.ValueObjects
{
    public sealed class FullName
    {
        public string FirstName { get; }
        public string LastName { get; }

        public FullName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new DomainValidationException("First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new DomainValidationException("Last name cannot be empty.");

            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString() => $"{FirstName} {LastName}";
        public override bool Equals(object? obj)
        {
            if (obj is not FullName other) return false;
            return FirstName == other.FirstName && LastName == other.LastName;
        }

        // adding this only for a better look up if need search in a list 
        public override int GetHashCode() => HashCode.Combine(FirstName, LastName);

    }
}
