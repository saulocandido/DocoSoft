using DOCOsoft.UserManagement.Application.Interfaces;
using DOCOsoft.UserManagement.Domain.Entities;


namespace DOCOsoft.UserManagement.Infrastructure.Persistence.RoleRepository
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
