using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Properties.Base;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class PropertyDocumentRepository
        : GenericRepository<PropertyDocument>, IPropertyDocumentRepository
    {
        public PropertyDocumentRepository(AppDbContext context) : base(context) { }

        public async Task<IReadOnlyList<PropertyDocumentDto>> GetByPropertyAsync(
            Guid propertyId,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .AsNoTracking()
                .Where(d => d.PropertyId == propertyId
                         && d.CompanyId == companyId)
                .OrderBy(d => d.DocumentType)
                .Select(d => new PropertyDocumentDto
                {
                    Id = d.Id,
                    DocumentType = d.DocumentType.ToString(),
                    DocumentName = d.DocumentName,
                    FileUrl = d.FileUrl,
                    DocumentNumber = d.DocumentNumber,
                    IssueDate = d.IssueDate,
                    ExpiryDate = d.ExpiryDate,
                    FileName = d.FileName,
                    FileSizeInBytes = d.FileSizeInBytes,
                    Notes = d.Notes
                })
                .ToListAsync(ct);

        public async Task<PropertyDocument?> GetByIdForCommandAsync(
            Guid id,
            Guid companyId,
            CancellationToken ct = default) =>
            await _dbSet
                .FirstOrDefaultAsync(d => d.Id == id
                                       && d.CompanyId == companyId, ct);
    }
}