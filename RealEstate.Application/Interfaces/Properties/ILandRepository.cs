using RealEstate.Domain.Entities.Properties;
using RealEstate.Shared.Common;
namespace RealEstate.Domain.Interfaces.Properties
{
    public interface ILandRepository : IGenericRepository<LandProperty>
    {
        Task<LandProperty?> GetByIdAsync(Guid id, Guid companyId);
        Task<LandProperty?> GetByIdWithDetailsAsync(Guid id, Guid companyId);
        Task<PagedResult<LandProperty>> GetPagedAsync(Guid companyId, int page, int pageSize);
        Task<bool> ExistsAsync(Guid id, Guid companyId);
        Task<bool> HasChildrenAsync(Guid landId, Guid companyId);
    }
}
