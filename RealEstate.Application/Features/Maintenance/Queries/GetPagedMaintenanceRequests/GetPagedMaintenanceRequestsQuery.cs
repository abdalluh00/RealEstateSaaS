using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.DTOs.Maintenance;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Queries.GetPagedMaintenanceRequests
{
    public sealed class GetPagedMaintenanceRequestsQuery
        : IRequest<ApiResponse<PagedResult<MaintenanceListDto>>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public MaintenanceStatus? Status { get; init; }
        public MaintenanceCategory? Category { get; init; }
        public MaintenancePriority? Priority { get; init; }
        public Guid? PropertyId { get; init; }
        public Guid? AssignedToId { get; init; }
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}