using Microsoft.EntityFrameworkCore;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;
using RealEstate.Infrastructure.Persistence;


namespace RealEstate.Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Client>> GetByCompanyAsync(Guid companyId) =>
            await _dbSet
                .Where(x => x.CompanyId == companyId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Client>> GetByLeadStatusAsync(Guid companyId, string status) =>
            await _dbSet
                .Where(x => x.CompanyId == companyId && x.LeadStatus == status)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

        public async Task<bool> PhoneExistsAsync(Guid companyId, string phone) =>
            await _dbSet.AnyAsync(x => x.CompanyId == companyId && x.Phone == phone);

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Clients.AnyAsync(x => x.Id == id);
        }
    }
}
