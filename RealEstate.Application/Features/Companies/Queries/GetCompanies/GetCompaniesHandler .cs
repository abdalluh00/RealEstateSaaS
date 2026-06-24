using MediatR;
using RealEstate.Application.Features.Companies.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Companies.Queries.GetCompanies
{
    public class GetCompaniesHandler : IRequestHandler<GetCompaniesQuery, ApiResponse<List<CompanyDto>>>
    {
        private readonly ICompanyRepository _repo;

        public GetCompaniesHandler(ICompanyRepository repo) => _repo = repo;

        public async Task<ApiResponse<List<CompanyDto>>> Handle(
            GetCompaniesQuery request,
            CancellationToken ct)
        {
            var companies = await _repo.GetAllCompaniesAsync();

            var result = companies.Select(c => new CompanyDto(
                c.Id, c.Name, c.Phone, c.Logo,
                c.SubscriptionPlan, c.SubscriptionExpiry,
                c.IsActive, c.IsExpired,
                c.TotalUsers, c.TotalProperties
            )).ToList();

            return ApiResponse<List<CompanyDto>>.Ok(result);
        }
    }
}
