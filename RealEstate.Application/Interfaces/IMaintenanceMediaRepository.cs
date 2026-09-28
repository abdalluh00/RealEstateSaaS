using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Interfaces
{
    public interface IMaintenanceMediaRepository : IGenericRepository<MaintenanceMedia>
    {
        Task<MaintenanceMedia?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }
}