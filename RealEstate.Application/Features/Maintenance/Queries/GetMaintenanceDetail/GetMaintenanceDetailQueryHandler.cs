using MediatR;
using RealEstate.Application.DTOs.Maintenance;
using RealEstate.Application.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Maintenance.Queries.GetMaintenanceDetail
{
    public sealed class GetMaintenanceDetailQueryHandler
        : IRequestHandler<GetMaintenanceDetailQuery, ApiResponse<MaintenanceDetailDto>>
    {
        private readonly IMaintenanceRepository _maintenance;

        public GetMaintenanceDetailQueryHandler(IMaintenanceRepository maintenance)
            => _maintenance = maintenance;

        public async Task<ApiResponse<MaintenanceDetailDto>> Handle(
            GetMaintenanceDetailQuery query,
            CancellationToken ct)
        {
            var result = await _maintenance.GetDetailByIdAsync(
                query.Id, query.CompanyId, ct);

            if (result is null)
                throw new NotFoundException("طلب الصيانة", query.Id);

            return ApiResponse<MaintenanceDetailDto>.Ok(result);
        }
    }
}