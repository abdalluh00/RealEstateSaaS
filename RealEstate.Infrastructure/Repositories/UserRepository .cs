using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;
namespace RealEstate.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<UserListItem>> GetByCompanyAsync(Guid companyId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new UserListItem
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email,
                    Phone = x.Phone,
                    Role = x.Role,
                    IsActive = x.IsActive,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await _dbSet
                .FirstOrDefaultAsync(x => x.Email == email);

        public async Task<bool> EmailExistsAsync(string email) =>
            await _dbSet
                .AsNoTracking()
                .AnyAsync(x => x.Email == email);

        public async Task<UserDetailItem?> GetWithCompanyAsync(Guid id) =>
            await _dbSet
          .AsNoTracking()
          .Where(x => x.Id == id)
          .Select(x => new UserDetailItem
          {
              Id = x.Id,
              FullName = x.FullName,
              Email = x.Email,
              Phone = x.Phone,
              Role = x.Role,
              IsActive = x.IsActive,
              CompanyId = x.CompanyId,
              CompanyName = x.Company.Name,
              CreatedAt = x.CreatedAt
          })
          .FirstOrDefaultAsync();

        public async Task<bool> ExistsInCompanyAsync(Guid userId, Guid companyId)
        {
            return await _dbSet.AnyAsync(x => x.Id == userId && x.CompanyId == companyId);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(x => x.Id == id);
        }
    }
}
