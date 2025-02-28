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
                throw new ArgumentException("First name cannot be empty.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be empty.");

            FirstName = firstName;
            LastName = lastName;
        }

        private FullName() { FirstName = null!; LastName = null!; }

        public override string ToString() => $"{FirstName} {LastName}";
        public override bool Equals(object? obj)
        {
            if (obj is not FullName other) return false;
            return FirstName == other.FirstName && LastName == other.LastName;
        }

        public override int GetHashCode() => (FirstName + LastName).GetHashCode();
    }
}
