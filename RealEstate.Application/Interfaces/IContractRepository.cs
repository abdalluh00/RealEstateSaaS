
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;
using RealEstate.Domain.ReadModels;
using RealEstate.Shared.Common;

namespace RealEstate.Domain.Interfaces
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        Task<Contract?> GetByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<Contract?> GetByIdForUpdateAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> ContractNumberExistsAsync(
            Guid companyId,
            string contractNumber,
            Guid? excludeId = null,
            CancellationToken ct = default);

        Task<bool> HasOpenContractForPropertyAsync(
            Guid companyId,
            Guid propertyId,
            Guid? excludeContractId = null,
            CancellationToken ct = default);

        Task<string> GenerateNextContractNumberAsync(
            Guid companyId,
            CancellationToken ct = default);

        Task<Contract?> GetDetailsAsync(
            Guid companyId,
            Guid contractId,
            CancellationToken ct = default);

        Task<List<ContractListItem>> GetByCompanyAsync(
            Guid companyId,
            CancellationToken ct = default);

        Task<PagedResult<Contract>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            ContractStatus? status = null,
            ContractType? contractType = null,
            CancellationToken ct = default);
    }
}

