using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Common.Extensions;
using RealEstate.Application.DTOs.Cheques;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;

namespace RealEstate.Infrastructure.Repositories
{
    public class ChequeRepository
        : GenericRepository<Cheque>, IChequeRepository
    {
        public ChequeRepository(AppDbContext context)
            : base(context)
        {
        }

        // ── Paged List ────────────────────────────────────
        public async Task<PagedResult<ChequeListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            Guid? contractId = null,
            ChequeStatus? status = null,
            DateTime? from = null,
            DateTime? to = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(c => c.CompanyId == companyId);

            if (contractId.HasValue)
            {
                query = query.Where(c =>
                    c.ContractId == contractId.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(c =>
                    c.Status == status.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(c =>
                    c.DueDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(c =>
                    c.DueDate <= to.Value);
            }

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
            {
                return PagedResult<ChequeListDto>
                    .Empty(page, pageSize);
            }

            /*
             * Important:
             *
             * Pagination happens BEFORE ToListAsync().
             * Therefore we only load the requested page.
             */
            var items = await query
                .OrderBy(c => c.DueDate)
                .ThenBy(c => c.ChequeOrder)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ChequeListProjection
                {
                    Id = c.Id,
                    ChequeNumber = c.ChequeNumber,
                    BankName = c.BankName,
                    Amount = c.Amount,
                    DueDate = c.DueDate,
                    ChequeOrder = c.ChequeOrder,
                    Status = c.Status,
                    DepositedAt = c.DepositedAt,
                    ClearedAt = c.ClearedAt,
                    BouncedAt = c.BouncedAt,
                    ContractId = c.ContractId,
                    ContractNumber = c.Contract.ContractNumber,
                    BounceReason = c.BounceReason,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(ct);

            var dtos = items
                .Select(c => new ChequeListDto
                {
                    Id = c.Id,
                    ChequeNumber = c.ChequeNumber,
                    BankName = c.BankName,
                    Amount = c.Amount,
                    DueDate = c.DueDate,
                    ChequeOrder = c.ChequeOrder,

                    Status = c.Status.ToArabicString(),

                    DepositedAt = c.DepositedAt,
                    ClearedAt = c.ClearedAt,
                    BouncedAt = c.BouncedAt,

                    ContractId = c.ContractId,
                    ContractNumber = c.ContractNumber,

                    BounceReason = c.BounceReason,
                    CreatedAt = c.CreatedAt
                })
                .ToList();

            return PagedResult<ChequeListDto>
                .Create(items: dtos, totalCount, page, pageSize);
        }

        // ── Detail ────────────────────────────────────────
        public async Task<ChequeDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default)
        {
            var cheque = await _dbSet
                .AsNoTracking()
                .Where(c =>
                    c.Id == id &&
                    c.CompanyId == companyId)
                .Select(c => new ChequeDetailProjection
                {
                    Id = c.Id,

                    ChequeNumber = c.ChequeNumber,
                    BankName = c.BankName,
                    Amount = c.Amount,
                    DueDate = c.DueDate,
                    ChequeOrder = c.ChequeOrder,

                    Status = c.Status,

                    DepositedAt = c.DepositedAt,
                    ClearedAt = c.ClearedAt,
                    BouncedAt = c.BouncedAt,
                    CancelledAt = c.CancelledAt,

                    BounceReason = c.BounceReason,
                    ReplacedByChequeId = c.ReplacedByChequeId,

                    Notes = c.Notes,

                    ContractId = c.ContractId,
                    ContractNumber = c.Contract.ContractNumber,

                    CompanyId = c.CompanyId,

                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .FirstOrDefaultAsync(ct);

            if (cheque is null)
                return null;

            return new ChequeDetailDto
            {
                Id = cheque.Id,

                ChequeNumber = cheque.ChequeNumber,
                BankName = cheque.BankName,
                Amount = cheque.Amount,
                DueDate = cheque.DueDate,
                ChequeOrder = cheque.ChequeOrder,

                Status = cheque.Status.ToArabicString(),

                DepositedAt = cheque.DepositedAt,
                ClearedAt = cheque.ClearedAt,
                BouncedAt = cheque.BouncedAt,
                CancelledAt = cheque.CancelledAt,

                BounceReason = cheque.BounceReason,
                ReplacedByChequeId = cheque.ReplacedByChequeId,

                Notes = cheque.Notes,

                ContractId = cheque.ContractId,
                ContractNumber = cheque.ContractNumber,

                CompanyId = cheque.CompanyId,

                CreatedAt = cheque.CreatedAt,
                UpdatedAt = cheque.UpdatedAt
            };
        }

        // ── By Contract ───────────────────────────────────
        public async Task<IReadOnlyList<ChequeListDto>> GetByContractAsync(
            Guid contractId,
            Guid companyId,
            CancellationToken ct = default)
        {
            var items = await _dbSet
                .AsNoTracking()
                .Where(c =>
                    c.ContractId == contractId &&
                    c.CompanyId == companyId)
                .OrderBy(c => c.ChequeOrder)
                .Select(c => new ChequeListProjection
                {
                    Id = c.Id,
                    ChequeNumber = c.ChequeNumber,
                    BankName = c.BankName,
                    Amount = c.Amount,
                    DueDate = c.DueDate,
                    ChequeOrder = c.ChequeOrder,
                    Status = c.Status,
                    DepositedAt = c.DepositedAt,
                    ClearedAt = c.ClearedAt,
                    BouncedAt = c.BouncedAt,
                    ContractId = c.ContractId,
                    ContractNumber = c.Contract.ContractNumber,
                    BounceReason = c.BounceReason,
                    CreatedAt = c.CreatedAt
                })
                .ToListAsync(ct);

            return items
                .Select(c => new ChequeListDto
                {
                    Id = c.Id,
                    ChequeNumber = c.ChequeNumber,
                    BankName = c.BankName,
                    Amount = c.Amount,
                    DueDate = c.DueDate,
                    ChequeOrder = c.ChequeOrder,

                    Status = c.Status.ToArabicString(),

                    DepositedAt = c.DepositedAt,
                    ClearedAt = c.ClearedAt,
                    BouncedAt = c.BouncedAt,

                    ContractId = c.ContractId,
                    ContractNumber = c.ContractNumber,

                    BounceReason = c.BounceReason,
                    CreatedAt = c.CreatedAt
                })
                .ToList();
        }

        // ── Validation ────────────────────────────────────
        public async Task<bool> ExistsInCompanyAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _dbSet.AnyAsync(
                c => c.Id == id &&
                     c.CompanyId == companyId,
                ct);
        }

        public async Task<bool> ChequeNumberExistsAsync(
            string chequeNumber,
            Guid companyId,
            Guid? excludeChequeId = null,
            CancellationToken ct = default)
        {
            var query = _dbSet.Where(c =>
                c.CompanyId == companyId &&
                c.ChequeNumber == chequeNumber);

            if (excludeChequeId.HasValue)
            {
                query = query.Where(c =>
                    c.Id != excludeChequeId.Value);
            }

            return await query.AnyAsync(ct);
        }
    }

    internal sealed class ChequeListProjection
    {
        public Guid Id { get; init; }

        public string ChequeNumber { get; init; } = string.Empty;
        public string BankName { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
        public int ChequeOrder { get; init; }

        public ChequeStatus Status { get; init; }

        public DateTime? DepositedAt { get; init; }
        public DateTime? ClearedAt { get; init; }
        public DateTime? BouncedAt { get; init; }

        public Guid ContractId { get; init; }
        public string ContractNumber { get; init; } = string.Empty;

        public string? BounceReason { get; init; }

        public DateTime CreatedAt { get; init; }
    }

    internal sealed class ChequeDetailProjection
    {
        public Guid Id { get; init; }

        public string ChequeNumber { get; init; } = string.Empty;
        public string BankName { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime DueDate { get; init; }
        public int ChequeOrder { get; init; }

        public ChequeStatus Status { get; init; }

        public DateTime? DepositedAt { get; init; }
        public DateTime? ClearedAt { get; init; }
        public DateTime? BouncedAt { get; init; }
        public DateTime? CancelledAt { get; init; }

        public string? BounceReason { get; init; }
        public Guid? ReplacedByChequeId { get; init; }

        public string? Notes { get; init; }

        public Guid ContractId { get; init; }
        public string ContractNumber { get; init; } = string.Empty;

        public Guid CompanyId { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}