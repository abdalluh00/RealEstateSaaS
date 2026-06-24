using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.ReadModels;
using RealEstate.Infrastructure.Persistence;


namespace RealEstate.Infrastructure.Repositories
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<CompanyListItem>> GetAllCompaniesAsync() => await _dbSet
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new CompanyListItem
            {
                Id = x.Id,
                Name = x.Name,
                Phone = x.Phone,
                Logo = x.Logo,
                SubscriptionPlan = x.SubscriptionPlan,
                SubscriptionExpiry = x.SubscriptionExpiry,
                IsActive = x.IsActive,
                TotalUsers = x.Users.Count,
                TotalProperties = x.Properties.Count
            })
            .ToListAsync();

        public async Task<CompanyDetailItem?> GetWithStatsAsync(Guid id) =>
            await _dbSet
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(x => new CompanyDetailItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    Phone = x.Phone,
                    Logo = x.Logo,
                    Address = x.Address,
                    SubscriptionPlan = x.SubscriptionPlan,
                    SubscriptionExpiry = x.SubscriptionExpiry,
                    IsActive = x.IsActive,
                    TotalUsers = x.Users.Count,
                    TotalProperties = x.Properties.Count,
                    TotalClients = x.Clients.Count,
                   
                })
                .FirstOrDefaultAsync();

        public async Task<bool> PhoneExistsAsync(string phone) =>
            await _dbSet
                .AsNoTracking()
                .AnyAsync(x => x.Phone == phone);
    }
}
