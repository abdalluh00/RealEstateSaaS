using RealEstate.Domain.Entities;
using RealEstate.Domain.ReadModels;

namespace RealEstate.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<IEnumerable<UserListItem>> GetByCompanyAsync(Guid companyId);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<UserDetailItem?> GetWithCompanyAsync(Guid id);
        Task<bool> ExistsInCompanyAsync(Guid userId, Guid companyId);
        Task<bool> ExistsAsync(Guid id);
    }
}
