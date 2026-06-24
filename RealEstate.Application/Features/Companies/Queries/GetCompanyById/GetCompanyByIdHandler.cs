using MediatR;
using RealEstate.Application.Features.Companies.DTO;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Companies.Queries.GetCompanyById
{
    public class GetCompanyByIdHandler
    : IRequestHandler<GetCompanyByIdQuery, ApiResponse<CompanyDetailDto>>
    {
        private readonly ICompanyRepository _repo;

        public GetCompanyByIdHandler(ICompanyRepository repo) => _repo = repo;

        public async Task<ApiResponse<CompanyDetailDto>> Handle(
            GetCompanyByIdQuery request,
            CancellationToken ct)
        {
            var company = await _repo.GetWithStatsAsync(request.Id);

            if (company is null)
                throw new NotFoundException("الشركة", request.Id);

            var result = new CompanyDetailDto(
                company.Id,
                company.Name,
                company.Phone,
                company.Logo,
                company.Address,
                company.SubscriptionPlan,
                company.SubscriptionExpiry,
                company.IsActive,
                company.IsExpired,
                company.TotalUsers,
                company.TotalProperties,
                company.TotalClients,
                company.TotalContracts,
                company.ActiveContracts
            );

            return ApiResponse<CompanyDetailDto>.Ok(result);
        }
    }
}
