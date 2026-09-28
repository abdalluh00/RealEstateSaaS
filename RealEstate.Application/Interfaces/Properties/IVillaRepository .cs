using RealEstate.Application.DTOs.Properties.Villa;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Interfaces.Properties
{
    public interface IVillaRepository : IGenericRepository<VillaProperty>
    {
        Task<PagedResult<VillaListDto>> GetPagedAsync(
            Guid companyId,
            int page,
            int pageSize,
            PropertyStatus? status = null,
            PropertyPurpose? purpose = null,
            int? minBedrooms = null,
            int? maxBedrooms = null,
            FurnishedStatus? furnishedStatus = null,
            bool? hasPool = null,
            CancellationToken ct = default);

        Task<VillaDetailDto?> GetDetailByIdAsync(
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