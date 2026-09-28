using RealEstate.Application.DTOs.Properties.Office;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface IOfficeRepository : IGenericRepository<OfficeProperty>
    {
        Task<PagedResult<OfficeListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            FurnishedStatus? furnishedStatus = null,
            CancellationToken ct = default);

        Task<OfficeDetailDto?> GetDetailByIdAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> IsAvailableAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> UnitNumberExistsAsync(
            string unitNumber,
            Guid parentPropertyId,
            CancellationToken ct = default);
    }
}