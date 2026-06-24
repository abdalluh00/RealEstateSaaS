using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;

namespace RealEstate.Infrastructure.Repositories
{
    public class OwnerRepository : GenericRepository<Owner>, IOwnerRepository
    {
        public OwnerRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<OwnerListItem>> GetByCompanyAsync(Guid companyId) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new OwnerListItem
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Phone = x.Phone,
                    Email = x.Email,
                    IdNumber = x.NationalId,
                    //TotalProperties = x.Properties.Count,
                    //ActiveContracts = x.Properties
                    //    .SelectMany(p => p.Contracts)
                    //    .Count(c => c.Status == "Active"),
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();

        public async Task<OwnerDetailItem?> GetWithPropertiesAsync(Guid id) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new OwnerDetailItem
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Phone = x.Phone,
                    Email = x.Email,
                    IdNumber = x.NationalId,
                    Notes = x.Notes,
                    //Properties = x.Properties.Select(p => new OwnerPropertyItem
                    //{
                    //    Id = p.Id,
                    //    Title = p.Title,
                    //    Type = p.Type,
                    //    Status = p.Status,
                    //    Price = p.Price,
                    //    City = p.City
                    //}).ToList()
                })
                .FirstOrDefaultAsync();

        public async Task<bool> PhoneExistsAsync(Guid companyId, string phone) =>
            await _dbSet
                .AsNoTracking()
                .AnyAsync(x => x.CompanyId == companyId && x.Phone == phone);

        public async Task<bool> ExistsInCompanyAsync(Guid ownerId, Guid companyId)
        {
            return await _dbSet.AnyAsync(x => x.Id == ownerId && x.CompanyId == companyId);
        }
    }
}
