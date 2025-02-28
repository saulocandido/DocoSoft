
namespace DOCOsoft.UserManagement.Domain.ValueObjects
{
    public sealed class Email
    {
        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email cannot be empty.");

            if (!value.Contains("@"))
                throw new ArgumentException("Email must contain '@' symbol.");

            Value = value;
        }

        private Email() { Value = null!; }

        public override string ToString() => Value;
        public override bool Equals(object? obj)
            => obj is Email other && Value == other.Value;
        public override int GetHashCode() => Value.GetHashCode();
    }
}
