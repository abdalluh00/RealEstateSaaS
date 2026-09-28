using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Maintenance;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class MaintenanceRepository
        : GenericRepository<MaintenanceRequest>, IMaintenanceRepository
    {
        public MaintenanceRepository(AppDbContext context) : base(context) { }

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<MaintenanceListDto>> GetPagedAsync(
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
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(m => m.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(m => m.Status == status.Value);

            if (category.HasValue)
                query = query.Where(m => m.Category == category.Value);

            if (priority.HasValue)
                query = query.Where(m => m.Priority == priority.Value);

            if (propertyId.HasValue)
                query = query.Where(m => m.PropertyId == propertyId.Value);

            if (assignedToId.HasValue)
                query = query.Where(m => m.AssignedToId == assignedToId.Value);

            if (dateFrom.HasValue)
                query = query.Where(m => m.CreatedAt >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(m => m.CreatedAt <= dateTo.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<MaintenanceListDto>.Empty(page, pageSize);

            // ── Project with joins via subqueries ─────────
            // No navigation properties on entity — use _context directly
            var items = await query
                .OrderByDescending(m => m.Priority)
                .ThenByDescending(m => m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MaintenanceListDto
                {
                    Id = m.Id,
                    RequestNumber = m.RequestNumber,
                    Title = m.Title,
                    Category = m.Category.ToArabicString(),
                    Priority = m.Priority.ToArabicString(),
                    Status = m.Status.ToArabicString(),
                    PropertyId = m.PropertyId,
                    PropertyTitle = m.PropertyId.HasValue
                        ? _context.Properties
                            .Where(p => p.Id == m.PropertyId.Value)
                            .Select(p => p.Title)
                            .FirstOrDefault()
                        : null,
                    ClientName = m.ClientId.HasValue
                        ? _context.Clients
                            .Where(c => c.Id == m.ClientId.Value)
                            .Select(c => c.FullName)
                            .FirstOrDefault()
                        : null,
                    AssignedToName = m.AssignedToId.HasValue
                        ? _context.Users
                            .Where(u => u.Id == m.AssignedToId.Value)
                            .Select(u => u.FullName)
                            .FirstOrDefault()
                        : null,
                    CreatedAt = m.CreatedAt
                })
                .ToListAsync(ct);

            return PagedResult<MaintenanceListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<MaintenanceDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(m => m.Id == id && m.CompanyId == companyId)
                .Select(m => new MaintenanceDetailDto
                {
                    Id = m.Id,
                    RequestNumber = m.RequestNumber,
                    Title = m.Title,
                    Description = m.Description,
                    Category = m.Category.ToArabicString(),
                    Priority = m.Priority.ToArabicString(),
                    Status = m.Status.ToArabicString(),
                    Cost = m.Cost,
                    ResolutionNotes = m.ResolutionNotes,
                    ResolvedAt = m.ResolvedAt,
                    ContractorName = m.ContractorName,
                    ContractorPhone = m.ContractorPhone,
                    PropertyId = m.PropertyId,
                    PropertyTitle = m.PropertyId.HasValue
                        ? _context.Properties
                            .Where(p => p.Id == m.PropertyId.Value)
                            .Select(p => p.Title)
                            .FirstOrDefault()
                        : null,
                    PropertyCode = m.PropertyId.HasValue
                        ? _context.Properties
                            .Where(p => p.Id == m.PropertyId.Value)
                            .Select(p => p.PropertyCode)
                            .FirstOrDefault()
                        : null,
                    ClientId = m.ClientId,
                    ClientName = m.ClientId.HasValue
                        ? _context.Clients
                            .Where(c => c.Id == m.ClientId.Value)
                            .Select(c => c.FullName)
                            .FirstOrDefault()
                        : null,
                    ClientPhone = m.ClientId.HasValue
                        ? _context.Clients
                            .Where(c => c.Id == m.ClientId.Value)
                            .Select(c => c.Phone)
                            .FirstOrDefault()
                        : null,
                    AssignedToId = m.AssignedToId,
                    AssignedToName = m.AssignedToId.HasValue
                        ? _context.Users
                            .Where(u => u.Id == m.AssignedToId.Value)
                            .Select(u => u.FullName)
                            .FirstOrDefault()
                        : null,
                    AssignedToPhone = m.AssignedToId.HasValue
                        ? _context.Users
                            .Where(u => u.Id == m.AssignedToId.Value)
                            .Select(u => u.Phone)
                            .FirstOrDefault()
                        : null,
                    Media = _context.MaintenanceMedias
                        .Where(mm => mm.MaintenanceRequestId == m.Id)
                        .Select(mm => new MaintenanceMediaDto
                        {
                            Id = mm.Id,
                            FileUrl = mm.FileUrl,
                            MediaType = mm.MediaType.ToString(),
                            Stage = mm.Stage.ToString(),
                            FileName = mm.FileName,
                            FileSizeInBytes = mm.FileSizeInBytes
                        })
                        .ToList(),
                    CreatedAt = m.CreatedAt,
                    UpdatedAt = m.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Commands ──────────────────────────────────────
        public async Task<MaintenanceRequest?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(m => m.Id == id
                                       && m.CompanyId == companyId, ct);
    }
}