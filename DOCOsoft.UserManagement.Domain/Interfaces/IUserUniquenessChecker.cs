using DOCOsoft.UserManagement.Domain.ValueObjects;


namespace DOCOsoft.UserManagement.Domain.Interfaces
{
    public interface IUserUniquenessChecker
    {
        bool IsUserEmailUnique(Email email);
    }
}
