

namespace DOCOsoft.UserManagement.Application.Users.Commands.UpdateUser
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(Guid userId)
            : base($"User with ID '{userId}' not found.") { }
    }

    public class DuplicateEmailException : Exception
    {
        public DuplicateEmailException(string email)
            : base($"Email '{email}' is already in use.") { }
    }

}
