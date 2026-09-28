using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Interfaces
{
    public interface IPropertyDocumentRepository : IGenericRepository<PropertyDocument>
    {
        Task<IReadOnlyList<PropertyDocumentDto>> GetByPropertyAsync(
            Guid propertyId,
            Guid companyId,
            CancellationToken ct = default);

        Task<PropertyDocument?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default);
    }
}