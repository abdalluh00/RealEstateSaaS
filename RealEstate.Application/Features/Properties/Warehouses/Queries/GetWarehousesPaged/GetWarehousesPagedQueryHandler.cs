using MediatR;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.ReadModels;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehousesPaged
{
    public class GetWarehousesPagedQueryHandler
        : IRequestHandler<GetWarehousesPagedQuery, ApiResponse<PagedResult<WarehouseListItemDto>>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehousesPagedQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ApiResponse<PagedResult<WarehouseListItemDto>>> Handle(GetWarehousesPagedQuery request, CancellationToken ct)
        {
            var result = await _warehouseRepository.GetPagedAsync(
                request.CompanyId,
                request.Page,
                request.PageSize,
                request.Search,
                ct);

            return ApiResponse<PagedResult<WarehouseListItemDto>>.Ok(result);
        }
    }
}
