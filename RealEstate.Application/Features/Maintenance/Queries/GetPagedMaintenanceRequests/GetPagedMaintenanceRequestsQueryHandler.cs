using MediatR;
using RealEstate.Application.DTOs.Maintenance;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Maintenance.Queries.GetPagedMaintenanceRequests
{
    public sealed class GetPagedMaintenanceRequestsQueryHandler
        : IRequestHandler<GetPagedMaintenanceRequestsQuery,
            ApiResponse<PagedResult<MaintenanceListDto>>>
    {
        private readonly IMaintenanceRepository _maintenance;

        public GetPagedMaintenanceRequestsQueryHandler(IMaintenanceRepository maintenance)
            => _maintenance = maintenance;

        public async Task<ApiResponse<PagedResult<MaintenanceListDto>>> Handle(
            GetPagedMaintenanceRequestsQuery query,
            CancellationToken ct)
        {
            var result = await _maintenance.GetPagedAsync(
                companyId: query.CompanyId,
                page: query.Page,
                pageSize: query.PageSize,
                status: query.Status,
                category: query.Category,
                priority: query.Priority,
                propertyId: query.PropertyId,
                assignedToId: query.AssignedToId,
                dateFrom: query.DateFrom,
                dateTo: query.DateTo,
                ct: ct);

            return ApiResponse<PagedResult<MaintenanceListDto>>.Ok(result);
        }
    }
}