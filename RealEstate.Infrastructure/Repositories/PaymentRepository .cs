using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Payments;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;
using System.Linq.Expressions;
using RealEstate.Application.Common.Extensions;

namespace RealEstate.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        private static readonly Expression<Func<Payment, PaymentListDto>> ToListDto =
            p => new PaymentListDto
            {
                Id = p.Id,
                PaymentNumber = p.PaymentNumber,
                Amount = p.Amount,
                DueDate = p.DueDate,
                PaidDate = p.PaidDate,
                PaymentStatus = p.PaymentStatus.ToArabicString(),
                PaymentMethod = p.PaymentMethod.ToArabicString(),
                Reference = p.Reference,
                Notes = p.Notes,
                ContractId = p.ContractId,
                CreatedAt = p.CreatedAt
            };

        // ── By Contract ───────────────────────────────────
        public async Task<IReadOnlyList<PaymentListDto>> GetByContractAsync(
            Guid contractId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(p => p.ContractId == contractId
                         && p.CompanyId == companyId)
                .OrderBy(p => p.PaymentNumber)
                .Select(ToListDto)
                .ToListAsync(ct);

        // ── Summary ───────────────────────────────────────
        public async Task<PaymentSummaryDto> GetContractSummaryAsync(
            Guid contractId,
            Guid companyId,
            CancellationToken ct = default)
        {
            var summary = await _dbSet
                .Where(p => p.ContractId == contractId
                         && p.CompanyId == companyId)
                .GroupBy(_ => 1)
                .Select(g => new PaymentSummaryDto
                {
                    ContractId = contractId,
                    TotalPayments = g.Count(),
                    PaidCount = g.Count(p => p.PaymentStatus == PaymentStatus.Paid),
                    PendingCount = g.Count(p => p.PaymentStatus == PaymentStatus.Pending),
                    OverdueCount = g.Count(p => p.PaymentStatus == PaymentStatus.Overdue),
                    CancelledCount = g.Count(p => p.PaymentStatus == PaymentStatus.Cancelled),
                    TotalAmount = g.Sum(p => p.Amount),
                    CollectedAmount = g.Sum(p => p.PaymentStatus == PaymentStatus.Paid
                        ? p.Amount : 0),
                    RemainingAmount = g.Sum(p => p.PaymentStatus != PaymentStatus.Paid
                                               && p.PaymentStatus != PaymentStatus.Cancelled
                        ? p.Amount : 0)
                })
                .FirstOrDefaultAsync(ct);

            return summary ?? new PaymentSummaryDto { ContractId = contractId };
        }

        // ── Paged ─────────────────────────────────────────
        public async Task<PagedResult<PaymentListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PaymentStatus? status = null,
            DateTime? dateFrom = null,
            DateTime? dateTo = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(p => p.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(p => p.PaymentStatus == status.Value);

            if (dateFrom.HasValue)
                query = query.Where(p => p.DueDate >= dateFrom.Value);

            if (dateTo.HasValue)
                query = query.Where(p => p.DueDate <= dateTo.Value);

            var totalCount = await query.CountAsync(ct);

            if (totalCount == 0)
                return PagedResult<PaymentListDto>.Empty(page, pageSize);

            var items = await query
                .OrderBy(p => p.DueDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(ToListDto)
                .ToListAsync(ct);

            return PagedResult<PaymentListDto>.Create(items, totalCount, page, pageSize);
        }

        // ── Commands ──────────────────────────────────────
        public async Task<Payment?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(p => p.Id == id
                                       && p.CompanyId == companyId, ct);

        // ── Background job ────────────────────────────────
        public async Task<int> MarkOverdueAsync(CancellationToken ct = default)
        {
            var today = DateTime.UtcNow.Date;

            var overduePayments = await _dbSet
                .Where(p => p.PaymentStatus == PaymentStatus.Pending
                         && p.DueDate.Date < today)
                .ToListAsync(ct);

            foreach (var payment in overduePayments)
                payment.PaymentStatus = PaymentStatus.Overdue;

            await _context.SaveChangesAsync(ct);

            return overduePayments.Count;
        }
    }
}