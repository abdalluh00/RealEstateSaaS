using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Clients;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context) { }

        // ── Reusable projection ───────────────────────────
        private static readonly Expression<Func<Client, ClientListDto>> ToListDto =
            c => new ClientListDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Phone = c.Phone,
                Email = c.Email,
                LeadStatus = c.LeadStatus.ToArabicString(),
                Source = c.Source.ToArabicString(),
                AssignedAgentName = c.AssignedAgent != null
                    ? c.AssignedAgent.FullName
                    : null,
                IsActive = c.IsActive,
                CreatedAt = c.CreatedAt
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<ClientListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            LeadStatus? leadStatus = null,
            LeadSource? source = null,
            bool? isActive = null,
            Guid? assignedAgentId = null,
            string? search = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(c => c.CompanyId == companyId);

            if (leadStatus.HasValue)
                query = query.Where(c => c.LeadStatus == leadStatus.Value);

            if (source.HasValue)
                query = query.Where(c => c.Source == source.Value);

            if (isActive.HasValue)
                query = query.Where(c => c.IsActive == isActive.Value);

            if (assignedAgentId.HasValue)
                query = query.Where(c => c.AssignedAgentId == assignedAgentId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c =>
                    c.FullName.Contains(search) ||
                    c.Phone.Contains(search));

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<ClientListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<ClientListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<ClientDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(c => c.Id == id && c.CompanyId == companyId)
                .Select(c => new ClientDetailDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    Phone = c.Phone,
                    Email = c.Email,
                    NationalId = c.NationalId,
                    Nationality = c.Nationality,
                    LeadStatus = c.LeadStatus.ToArabicString(),
                    Source = c.Source.ToArabicString(),
                    IsActive = c.IsActive,
                    Notes = c.Notes,
                    AssignedAgentId = c.AssignedAgentId,
                    AssignedAgentName = c.AssignedAgent != null
                        ? c.AssignedAgent.FullName
                        : null,
                    AssignedAgentPhone = c.AssignedAgent != null
                        ? c.AssignedAgent.Phone
                        : null,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Validation ────────────────────────────────────
        public async Task<bool> PhoneExistsAsync(
            string phone,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(c => c.Phone == phone
                            && c.CompanyId == companyId, ct);

        public async Task<bool> PhoneExistsForAnotherClientAsync(
            string phone,
            Guid clientId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(c => c.Phone == phone
                            && c.CompanyId == companyId
                            && c.Id != clientId, ct);

        public async Task<bool> ExistsInCompanyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(c => c.Id == id
                            && c.CompanyId == companyId, ct);
    }
}