using DOCOsoft.UserManagement.Domain.Entities;

namespace DOCOsoft.UserManagement.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<bool> IsEmailUniqueAsync(string email);
        Task<List<User>> GetAllUsersAsync();

        Task<User?> GetUserByIdAsync(Guid id);
    }
}
