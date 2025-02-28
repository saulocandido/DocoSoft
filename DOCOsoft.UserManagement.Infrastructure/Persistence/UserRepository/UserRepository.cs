using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace DOCOsoft.UserManagement.Infrastructure.Persistence.UserRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return !await _dbContext.Users.AnyAsync(u => u.Email.Value == email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users
                .Include(u => u.Roles)  
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _dbContext.Users
                .Include(u => u.Roles) 
                .FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
