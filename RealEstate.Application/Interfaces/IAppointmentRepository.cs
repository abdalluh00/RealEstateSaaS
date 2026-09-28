using RealEstate.Application.DTOs.Appointments;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        // ── Queries ───────────────────────────────────────
        Task<PagedResult<AppointmentListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            Guid? agentId = null,
            Guid? clientId = null,
            Guid? propertyId = null,
            AppointmentStatus? status = null,
            DateTime? from = null,
            DateTime? to = null,
            CancellationToken ct = default);

        Task<AppointmentDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<IReadOnlyList<AppointmentListDto>> GetTodayAsync(
            Guid companyId,
            Guid? agentId = null,
            CancellationToken ct = default);

        // ── Validation ────────────────────────────────────
        Task<bool> HasConflictAsync(
            Guid agentId,
            DateTime scheduledAt,
            int durationMinutes,
            Guid? excludeAppointmentId = null,
            CancellationToken ct = default);

        Task<bool> ExistsInCompanyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }
}