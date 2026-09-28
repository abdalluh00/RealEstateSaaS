using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Maintenance;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Queries.GetMaintenanceDetail
{
    public sealed class GetMaintenanceDetailQuery
        : IRequest<ApiResponse<MaintenanceDetailDto>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}