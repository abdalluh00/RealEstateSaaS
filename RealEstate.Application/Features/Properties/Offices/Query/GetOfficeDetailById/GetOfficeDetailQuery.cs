
using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Properties.Office;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Offices.Query.GetOfficeDetailById
{
    public sealed record GetOfficeDetailQuery : IRequest<ApiResponse<OfficeDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
