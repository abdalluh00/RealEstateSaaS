using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PaymentListItem>> GetByContractAsync(Guid contractId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.ContractId == contractId)
                .OrderBy(x => x.DueDate)
                .Select(x => new PaymentListItem
                {
                    Id = x.Id,
                    PropertyTitle = x.Contract.Property.Title,
                    ClientName = x.Contract.Client.FullName,
                    ClientPhone = x.Contract.Client.Phone,
                    Amount = x.Amount,
                    DueDate = x.DueDate,
                    PaidDate = x.PaidDate,
                    Status = x.Status,
                    Method = x.Method,
                    Reference = x.Reference,
                    ContractId = x.ContractId
                })
                .ToListAsync();

        public async Task<IEnumerable<PaymentListItem>> GetOverdueAsync(Guid companyId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x =>
                    x.Status == "Pending" &&
                    x.DueDate < DateTime.UtcNow &&
                    x.Contract.Property.CompanyId == companyId)
                .OrderBy(x => x.DueDate)
                .Select(x => new PaymentListItem
                {
                    Id = x.Id,
                    PropertyTitle = x.Contract.Property.Title,
                    ClientName = x.Contract.Client.FullName,
                    ClientPhone = x.Contract.Client.Phone,
                    Amount = x.Amount,
                    DueDate = x.DueDate,
                    PaidDate = x.PaidDate,
                    Status = x.Status,
                    Method = x.Method,
                    Reference = x.Reference,
                    ContractId = x.ContractId
                })
                .ToListAsync();

        public async Task<IEnumerable<PaymentListItem>> GetUpcomingAsync(
            Guid companyId, int daysAhead)
        {
            var targetDate = DateTime.UtcNow.AddDays(daysAhead);
            return await _dbSet
                .AsNoTracking()
                .Where(x =>
                    x.Status == "Pending" &&
                    x.DueDate >= DateTime.UtcNow &&
                    x.DueDate <= targetDate &&
                    x.Contract.Property.CompanyId == companyId)
                .OrderBy(x => x.DueDate)
                .Select(x => new PaymentListItem
                {
                    Id = x.Id,
                    PropertyTitle = x.Contract.Property.Title,
                    ClientName = x.Contract.Client.FullName,
                    ClientPhone = x.Contract.Client.Phone,
                    Amount = x.Amount,
                    DueDate = x.DueDate,
                    PaidDate = x.PaidDate,
                    Status = x.Status,
                    Method = x.Method,
                    Reference = x.Reference,
                    ContractId = x.ContractId
                })
                .ToListAsync();
        }

        public async Task<PaymentSummary> GetSummaryAsync(Guid companyId)
        {
            var now = DateTime.UtcNow;
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.Contract.Property.CompanyId == companyId)
                .GroupBy(x => 1)
                .Select(g => new PaymentSummary
                {
                    TotalExpected = g.Sum(x => x.Amount),
                    TotalCollected = g.Where(x => x.Status == "Paid").Sum(x => x.Amount),
                    TotalOverdue = g.Where(x => x.Status == "Pending" && x.DueDate < now).Sum(x => x.Amount),
                    TotalPending = g.Where(x => x.Status == "Pending" && x.DueDate >= now).Sum(x => x.Amount),
                    OverdueCount = g.Count(x => x.Status == "Pending" && x.DueDate < now),
                    PendingCount = g.Count(x => x.Status == "Pending" && x.DueDate >= now)
                })
                .FirstOrDefaultAsync() ?? new PaymentSummary();
        }
    }
}
