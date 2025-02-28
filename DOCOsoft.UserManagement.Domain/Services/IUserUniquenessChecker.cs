using DOCOsoft.UserManagement.Domain.ValueObjects;


namespace DOCOsoft.UserManagement.Domain.Services
{
    public interface IUserUniquenessChecker
    {
        bool IsUserEmailUnique(Email email);
    }
}
