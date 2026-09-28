using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Contracts;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext context) : base(context) { }

        // ── Reusable projection ───────────────────────────
        private static readonly Expression<Func<Contract, ContractListDto>> ToListDto =
            c => new ContractListDto
            {
                Id = c.Id,
                ContractNumber = c.ContractNumber,
                ContractType = c.ContractType.ToString(),
                ContractStatus = c.ContractStatus.ToString(),
                Amount = c.Amount,
                PaymentCycle = c.PaymentCycle.ToString(),
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                PropertyId = c.PropertyId,
                PropertyCode = c.Property.PropertyCode,
                PropertyTitle = c.Property.Title,
                PropertyCity = c.Property.City,
                ClientId = c.ClientId,
                ClientName = c.Client.FullName,
                ClientPhone = c.Client.Phone,
                AgentId = c.AgentId,
                AgentName = c.Agent.FullName,
                CommissionStatus = c.CommissionStatus.ToString(),
                CreatedAt = c.CreatedAt
            };

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<ContractListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            ContractStatus? status = null,
            ContractType? contractType = null,
            Guid? agentId = null,
            Guid? clientId = null,
            Guid? propertyId = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(c => c.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(c => c.ContractStatus == status.Value);

            if (contractType.HasValue)
                query = query.Where(c => c.ContractType == contractType.Value);

            if (agentId.HasValue)
                query = query.Where(c => c.AgentId == agentId.Value);

            if (clientId.HasValue)
                query = query.Where(c => c.ClientId == clientId.Value);

            if (propertyId.HasValue)
                query = query.Where(c => c.PropertyId == propertyId.Value);

            if (dateFrom.HasValue)
                query = query.Where(c => c.StartDate >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(c => c.StartDate <= dateTo.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<ContractListDto>.Empty(page, pageSize);

            var items = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<ContractListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<ContractDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(c => c.Id == id && c.CompanyId == companyId)
                .Select(c => new ContractDetailDto
                {
                    Id = c.Id,
                    ContractNumber = c.ContractNumber,
                    ContractType = c.ContractType.ToString(),
                    ContractStatus = c.ContractStatus.ToString(),
                    Amount = c.Amount,
                    SecurityDeposit = c.SecurityDeposit,
                    PaymentCycle = c.PaymentCycle.ToString(),
                    PaymentMethod = c.PaymentMethod.ToString(),
                    CommissionType = c.CommissionType.ToString(),
                    Commission = c.Commission,
                    CommissionStatus = c.CommissionStatus.ToString(),
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    CancelledAt = c.CancelledAt,
                    CancellationReason = c.CancellationReason,
                    Notes = c.Notes,
                    RenewedFromContractId = c.RenewedFromContractId,

                    // ── Renewal chain ─────────────────────
                    // Load previous contract number without
                    // navigation collection on Contract entity
                    RenewedFromContractNumber = c.RenewedFromContract != null
                        ? c.RenewedFromContract.ContractNumber
                        : null,

                    PropertyId = c.PropertyId,
                    PropertyCode = c.Property.PropertyCode,
                    PropertyTitle = c.Property.Title,
                    PropertyCity = c.Property.City,
                    PropertyType = c.Property.Type.ToArabicString(),
                    ClientId = c.ClientId,
                    ClientName = c.Client.FullName,
                    ClientPhone = c.Client.Phone,
                    ClientEmail = c.Client.Email,
                    AgentId = c.AgentId,
                    AgentName = c.Agent.FullName,
                    AgentPhone = c.Agent.Phone,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

        // ── Commands ──────────────────────────────────────
        public async Task<Contract?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(c => c.Id == id
                                       && c.CompanyId == companyId, ct);

        // ── Validation ────────────────────────────────────
        public async Task<bool> HasOpenContractForPropertyAsync(
            Guid propertyId,
            Guid companyId,
            Guid? excludeContractId = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .Where(c => c.PropertyId == propertyId
                         && c.CompanyId == companyId
                         && c.ContractStatus == ContractStatus.Active);

            if (excludeContractId.HasValue)
                query = query.Where(c => c.Id != excludeContractId.Value);

            return await query.AnyAsync(ct);
        }
    }
}