using RealEstate.Domain.Entities;
namespace RealEstate.Domain.Interfaces
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<IEnumerable<Client>> GetByCompanyAsync(Guid companyId);
        Task<IEnumerable<Client>> GetByLeadStatusAsync(Guid companyId, string status);
        Task<bool> PhoneExistsAsync(Guid companyId, string phone);
        Task<bool> ExistsAsync(Guid id);
    }
}
