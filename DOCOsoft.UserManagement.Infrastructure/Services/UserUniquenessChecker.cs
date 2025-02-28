using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Services;
using DOCOsoft.UserManagement.Domain.ValueObjects;


namespace DOCOsoft.UserManagement.Infrastructure.Services
{
    public class UserUniquenessChecker : IUserUniquenessChecker
    {
        private readonly IUserUniquenessCheckerRepo _repo;
        public UserUniquenessChecker(IUserUniquenessCheckerRepo repo)
        {
            _repo = repo;
        }

        public bool IsUserEmailUnique(Email email)
        {
            return _repo.IsEmailUnique(email.Value);
        }
    }
}
