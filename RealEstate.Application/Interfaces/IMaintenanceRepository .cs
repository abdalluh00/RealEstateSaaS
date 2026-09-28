using RealEstate.Application.DTOs.Maintenance;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IMaintenanceRepository : IGenericRepository<MaintenanceRequest>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<MaintenanceListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            MaintenanceStatus? status = null,
            MaintenanceCategory? category = null,
            MaintenancePriority? priority = null,
            Guid? propertyId = null,
            Guid? assignedToId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            CancellationToken ct = default);

        Task<MaintenanceDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        // ── Commands ──────────────────────────────────────
        Task<MaintenanceRequest?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }
}