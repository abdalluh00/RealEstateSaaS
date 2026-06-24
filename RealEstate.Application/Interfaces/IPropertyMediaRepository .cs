using RealEstate.Domain.Entities;
namespace RealEstate.Domain.Interfaces
{
    public interface IPropertyMediaRepository : IGenericRepository<PropertyMedia>
    {
        Task<IEnumerable<PropertyMedia>> GetByPropertyAsync(Guid propertyId);
        Task<PropertyMedia?> GetCoverAsync(Guid propertyId);
        Task RemoveAllByPropertyAsync(Guid propertyId);
    }
}
