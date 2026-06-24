using RealEstate.Domain.Entities;
using RealEstate.Domain.ReadModels;

namespace RealEstate.Domain.Interfaces
{
    public interface IOwnerRepository : IGenericRepository<Owner>
    {
        Task<IEnumerable<OwnerListItem>> GetByCompanyAsync(Guid companyId);
        Task<OwnerDetailItem?> GetWithPropertiesAsync(Guid id);
        Task<bool> PhoneExistsAsync(Guid companyId, string phone);
        Task<bool> ExistsInCompanyAsync(Guid ownerId, Guid companyId);
    }
}
