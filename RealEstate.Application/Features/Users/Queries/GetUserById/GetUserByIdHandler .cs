using MediatR;
using RealEstate.Application.DTOs.Companies;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Companies.Queries.GetCompany
{
    public sealed class GetCompanyQueryHandler
        : IRequestHandler<GetCompanyQuery, ApiResponse<CompanyDto>>
    {
        private readonly ICompanyRepository _companies;

        public GetCompanyQueryHandler(ICompanyRepository companies)
            => _companies = companies;

        public async Task<ApiResponse<CompanyDto>> Handle(
            GetCompanyQuery query,
            CancellationToken ct)
        {
            var company = await _companies.GetDetailByIdAsync(
                query.CompanyId, ct);

            if (company is null)
                throw new NotFoundException("الشركة", query.CompanyId);

            return ApiResponse<CompanyDto>.Ok(company);
        }
    }
}