using DOCOsoft.UserManagement.Application.Interfaces;


namespace DOCOsoft.UserManagement.Infrastructure.Persistence.UserRepository
{
    public class UserUniquenessCheckerRepo : IUserUniquenessCheckerRepo
    {
        private readonly AppDbContext _db;
        public UserUniquenessCheckerRepo(AppDbContext db)
        {
            _db = db;
        }

        public bool IsEmailUnique(string emailValue)
        {
            return !_db.Users.Any(u => u.Email.Value == emailValue);
        }
    }
}
