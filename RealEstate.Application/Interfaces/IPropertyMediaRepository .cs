using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyMediaRepository : IGenericRepository<PropertyMedia>
    {
        Task<IReadOnlyList<PropertyMediaDto>> GetByPropertyAsync(
            Guid propertyId,
            Guid companyId,
            CancellationToken ct = default);

        Task<PropertyMedia?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);

        Task<bool> HasCoverAsync(
            Guid propertyId,
            CancellationToken ct = default);

        Task ClearCoverAsync(
            Guid propertyId,
            CancellationToken ct = default);
    }
}