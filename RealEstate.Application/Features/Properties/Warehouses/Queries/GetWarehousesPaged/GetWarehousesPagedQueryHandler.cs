using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.DTOs.Properties.Warehouse;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehousesPaged
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public sealed class GetWarehousesPagedQueryHandler
        : IRequestHandler<GetWarehousesPagedQuery, ApiResponse<PagedResult<WarehouseListDto>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehousesPagedQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ApiResponse<PagedResult<WarehouseListDto>>> Handle(GetWarehousesPagedQuery request, CancellationToken ct)
        {
            var result = await _warehouseRepository.GetPagedAsync(
                request.CompanyId,
                request.Page,
                request.PageSize,
                request.status,
                request.purpose);

            
            return ApiResponse<PagedResult<WarehouseListDto>>.Ok(result, "Warehouses retrieved successfully.");
        }
    }
}
