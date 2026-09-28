using RealEstate.Application.DTOs.Properties.Warehouse;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface IWarehouseRepository : IGenericRepository<WarehouseProperty>
    {
        Task<PagedResult<WarehouseListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            bool? hasColdstorage = null,
            CancellationToken ct = default);

        Task<WarehouseDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default);
    }
}