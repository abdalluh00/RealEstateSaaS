using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Companies;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Companies.Queries.GetCompany
{
    public sealed class GetCompanyQuery
        : IRequest<ApiResponse<CompanyDto>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}