

namespace DOCOsoft.UserManagement.Application.Interfaces
{
    public interface IUserUniquenessCheckerRepo
    {
        bool IsEmailUnique(string emailValue);
    }
}
