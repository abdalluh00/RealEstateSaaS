using RealEstate.Application.DTOs.Companies;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Interfaces;

namespace RealEstate.Application.Interfaces
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<CompanyDto?> GetDetailByIdAsync(
            Guid id,
            CancellationToken ct = default);

        Task<Company?> GetByIdForCommandAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> IsActiveAsync(
            Guid id,
            CancellationToken ct = default);

        Task<bool> IsSubscriptionValidAsync(
            Guid id,
            CancellationToken ct = default);
    }
}