using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyMediaRepository
        : GenericRepository<PropertyMedia>, IPropertyMediaRepository
    {
        public PropertyMediaRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<PropertyMediaDto>> GetByPropertyAsync(
            Guid propertyId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(m => m.PropertyId == propertyId
                         && m.CompanyId == companyId)
                .OrderByDescending(m => m.IsCover)
                .ThenBy(m => m.SortOrder)
                .Select(m => new PropertyMediaDto
                {
                    Id = m.Id,
                    MediaUrl = m.MediaUrl,
                    MediaType = m.MediaType.ToString(),
                    Title = m.Title,
                    Description = m.Description,
                    IsCover = m.IsCover,
                    SortOrder = m.SortOrder,
                    FileName = m.FileName,
                    FileSizeInBytes = m.FileSizeInBytes
                })
                .ToListAsync(ct);

        public async Task<PropertyMedia?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(m => m.Id == id
                                       && m.CompanyId == companyId, ct);

        public async Task<bool> HasCoverAsync(
            Guid propertyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AnyAsync(m => m.PropertyId == propertyId
                            && m.IsCover, ct);

        public async Task ClearCoverAsync(
            Guid propertyId,
            CancellationToken ct = default)
        {
            // Remove IsCover flag from all media of this property
            var covers = await _dbSet
                .Where(m => m.PropertyId == propertyId && m.IsCover)
                .ToListAsync(ct);

            foreach (var cover in covers)
                cover.IsCover = false;
        }
    }
}