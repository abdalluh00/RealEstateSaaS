using RealEstate.Domain.Entities.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Domain.Interfaces.Properties
{
    public interface IBuildingRepository : IGenericRepository<BuildingProperty>
    {
        Task<BuildingProperty?> GetByIdAsync(Guid id, Guid companyId);
        Task<BuildingProperty?> GetByIdWithDetailsAsync(Guid id, Guid companyId);
        Task<PagedResult<BuildingProperty>> GetPagedAsync(Guid companyId, int page, int pageSize);
        Task<bool> ExistsAsync(Guid id, Guid companyId);
        Task<bool> HasChildrenAsync(Guid buildingId, Guid companyId);
    }
}
