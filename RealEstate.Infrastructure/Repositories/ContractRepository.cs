using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;
using RealEstate.Shared.Common;

namespace RealEstate.Infrastructure.Repositories
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Contract?> GetByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId, ct);
        }

        public async Task<Contract?> GetByIdForUpdateAsync(
            Guid id,
            CancellationToken ct = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<bool> ContractNumberExistsAsync(
            Guid companyId,
            string contractNumber,
            Guid? excludeId = null,
            CancellationToken ct = default)
        {
            var query = _dbSet.Where(x =>
                x.CompanyId == companyId &&
                x.ContractNumber == contractNumber);

            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            return await query.AnyAsync(ct);
        }

        public async Task<bool> HasOpenContractForPropertyAsync(
            Guid companyId,
            Guid propertyId,
            Guid? excludeContractId = null,
            CancellationToken ct = default)
        {
            var query = _dbSet.Where(x =>
                x.CompanyId == companyId &&
                x.PropertyId == propertyId &&
                (x.ContractStatus == ContractStatus.Active ||
                 x.ContractStatus == ContractStatus.Renewed));

            if (excludeContractId.HasValue)
                query = query.Where(x => x.Id != excludeContractId.Value);

            return await query.AnyAsync(ct);
        }

        public async Task<string> GenerateNextContractNumberAsync(
            Guid companyId,
            CancellationToken ct = default)
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"CTR-{year}-";

            var contractNumbers = await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId && x.ContractNumber.StartsWith(prefix))
                .Select(x => x.ContractNumber)
                .ToListAsync(ct);

            var maxNumber = 0;

            foreach (var number in contractNumbers)
            {
                var suffix = number.Replace(prefix, "");
                if (int.TryParse(suffix, out var parsed) && parsed > maxNumber)
                    maxNumber = parsed;
            }

            return $"{prefix}{(maxNumber + 1):D5}";
        }

        public async Task<Contract?> GetDetailsAsync(
            Guid companyId,
            Guid contractId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.Property)
                .Include(x => x.Client)
                .Include(x => x.Agent)
                .FirstOrDefaultAsync(
                    x => x.Id == contractId && x.CompanyId == companyId,
                    ct);
        }

        public async Task<List<ContractListItem>> GetByCompanyAsync(
            Guid companyId,
            CancellationToken ct = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new ContractListItem
                {
                    Id = x.Id,
                    ContractNumber = x.ContractNumber,
                    PropertyTitle = x.Property.Title,
                    ClientName = x.Client.FullName,
                    ClientPhone = x.Client.Phone,
                    AgentName = x.Agent.FullName,
                    ContractType = x.ContractType.ToString(),
                    Status = x.ContractStatus.ToString(),
                    Amount = x.Amount,
                    Commission = x.Commission,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate!.Value,

                    // currently no payments integration here yet
                    TotalPayments = 0,
                    PaidPayments = 0,
                    TotalPaid = 0
                    
                })
                .ToListAsync(ct);
        }

        public async Task<PagedResult<Contract>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            ContractStatus? status = null,
            ContractType? contractType = null,
            CancellationToken ct = default)
        {
            var query = _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId);

            if (status.HasValue)
                query = query.Where(x => x.ContractStatus == status.Value);

            if (contractType.HasValue)
                query = query.Where(x => x.ContractType == contractType.Value);

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Contract>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
