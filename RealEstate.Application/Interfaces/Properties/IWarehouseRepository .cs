using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.ReadModels;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Interfaces.Properties
{
    public interface IWarehouseRepository : IGenericRepository<WarehouseProperty>
    {
        Task<WarehouseProperty?> GetByIdForUpdateAsync(Guid id, Guid companyId, CancellationToken ct = default);

        Task<WarehouseDetailsDto?> GetDetailsByIdAsync(Guid id, Guid companyId, CancellationToken ct = default);

        Task<PagedResult<WarehouseListItemDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            string? search = null,
            CancellationToken ct = default);
    }
}
