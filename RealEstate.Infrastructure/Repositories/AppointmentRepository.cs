using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Appointments;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(AppDbContext context) : base(context) { }

        // ── Reusable projection ───────────────────────────
        private static readonly Expression<Func<Appointment, AppointmentListDto>> ToListDto =
            a => new AppointmentListDto
            {
                Id = a.Id,
                ScheduledAt = a.ScheduledAt,
                Status = a.Status.ToArabicString(),
                DurationMinutes = a.DurationMinutes,
                PropertyId = a.PropertyId,
                PropertyCode = a.Property.PropertyCode,
                PropertyTitle = a.Property.Title,
                PropertyCity = a.Property.City,
                ClientId = a.ClientId,
                ClientName = a.Client.FullName,
                ClientPhone = a.Client.Phone,
                AgentId = a.AgentId,
                AgentName = a.Agent.FullName,
                Result = a.Result.ToString(),
                CreatedAt = a.CreatedAt
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<AppointmentListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            Guid? agentId = null,
            Guid? clientId = null,
            Guid? propertyId = null,
            AppointmentStatus? status = null,
            DateTime? from = null,
            DateTime? to = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(a => a.CompanyId == companyId);

            if (agentId.HasValue)
                query = query.Where(a => a.AgentId == agentId.Value);

            if (clientId.HasValue)
                query = query.Where(a => a.ClientId == clientId.Value);

            if (propertyId.HasValue)
                query = query.Where(a => a.PropertyId == propertyId.Value);

            if (status.HasValue)
                query = query.Where(a => a.Status == status.Value);

            if (from.HasValue)
                query = query.Where(a => a.ScheduledAt >= from.Value);

            if (to.HasValue)
                query = query.Where(a => a.ScheduledAt <= to.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<AppointmentListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(a => a.ScheduledAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<AppointmentListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<AppointmentDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(a => a.Id == id && a.CompanyId == companyId)
                .Select(a => new AppointmentDetailDto
                {
                    Id = a.Id,
                    ScheduledAt = a.ScheduledAt,
                    ActualVisitAt = a.ActualVisitAt,
                    DurationMinutes = a.DurationMinutes,
                    Status = a.Status.ToArabicString(),
                    Notes = a.Notes,
                    Feedback = a.Feedback,
                    Result = a.Result.ToString(),
                    CancellationReason = a.CancellationReason,
                    CancelledAt = a.CancelledAt,
                    PropertyId = a.PropertyId,
                    PropertyCode = a.Property.PropertyCode,
                    PropertyTitle = a.Property.Title,
                    PropertyCity = a.Property.City,
                    PropertyDistrict = a.Property.District,
                    PropertyType = a.Property.Type.ToArabicString(),
                    ClientId = a.ClientId,
                    ClientName = a.Client.FullName,
                    ClientPhone = a.Client.Phone,
                    ClientEmail = a.Client.Email,
                    AgentId = a.AgentId,
                    AgentName = a.Agent.FullName,
                    AgentPhone = a.Agent.Phone,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Today ─────────────────────────────────────────
        public async Task<IReadOnlyList<AppointmentListDto>> GetTodayAsync(
            Guid companyId,
            Guid? agentId = null,
            CancellationToken ct = default)
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var query = _dbSet
                .AsNoTracking()
                .Where(a => a.CompanyId == companyId
                         && a.ScheduledAt >= today
                         && a.ScheduledAt < tomorrow
                         && a.Status != AppointmentStatus.Cancelled);

            if (agentId.HasValue)
                query = query.Where(a => a.AgentId == agentId.Value);

            return await query
                .OrderBy(a => a.ScheduledAt)
                .Select(ToListDto)
                .ToListAsync(ct);
        }

        // ── Conflict Check ────────────────────────────────
        public async Task<bool> HasConflictAsync(
            Guid agentId,
            DateTime scheduledAt,
            int durationMinutes,
            Guid? excludeAppointmentId = null,
            CancellationToken ct = default)
        {
            var newEnd = scheduledAt.AddMinutes(durationMinutes);

            var query = _dbSet
                .Where(a => a.AgentId == agentId
                         && a.Status != AppointmentStatus.Cancelled
                         && a.Status != AppointmentStatus.NoShow);

            if (excludeAppointmentId.HasValue)
                query = query.Where(a => a.Id != excludeAppointmentId.Value);

            // Conflict when time windows overlap:
            // existing.Start < new.End AND existing.End > new.Start
            return await query.AnyAsync(a =>
                a.ScheduledAt < newEnd &&
                a.ScheduledAt.AddMinutes(a.DurationMinutes ?? 30) > scheduledAt, ct);
        }

        // ── Validation ────────────────────────────────────
        public async Task<bool> ExistsInCompanyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(a => a.Id == id && a.CompanyId == companyId, ct);
    }
}