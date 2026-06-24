using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;
namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyMediaRepository : GenericRepository<PropertyMedia>, IPropertyMediaRepository
    {
        public PropertyMediaRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<PropertyMedia>> GetByPropertyAsync(Guid propertyId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.PropertyId == propertyId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

        public async Task<PropertyMedia?> GetCoverAsync(Guid propertyId) =>
            await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PropertyId == propertyId && x.IsCover);

        public async Task RemoveAllByPropertyAsync(Guid propertyId)
        {
            var media = await _dbSet
                .Where(x => x.PropertyId == propertyId)
                .ToListAsync();

            _dbSet.RemoveRange(media);
        }
    }
}
