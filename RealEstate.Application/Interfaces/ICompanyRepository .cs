using RealEstate.Domain.Entities;
using RealEstate.Domain.ReadModels;

namespace RealEstate.Domain.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<IEnumerable<CompanyListItem>> GetAllCompaniesAsync();
        Task<CompanyDetailItem?> GetWithStatsAsync(Guid id);
        Task<bool> PhoneExistsAsync(string phone);
    }
}
