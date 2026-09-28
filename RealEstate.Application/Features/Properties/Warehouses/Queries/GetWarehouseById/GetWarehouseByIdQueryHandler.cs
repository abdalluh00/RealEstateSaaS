using MediatR;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using RealEstate.Application.DTOs.Properties.Warehouse;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehouseById
{

    public class GetWarehouseByIdQueryHandler
        : IRequestHandler<GetWarehouseByIdQuery, ApiResponse<WarehouseDetailDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }
        async Task<ApiResponse<WarehouseDetailDto>> IRequestHandler<GetWarehouseByIdQuery, ApiResponse<WarehouseDetailDto>>.Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
        {
            var warehouse = await _warehouseRepository.GetDetailByIdAsync(request.Id, request.CompanyId, cancellationToken);
            if (warehouse == null)
            {
                throw new NotFoundException($"Warehouse with Id {request.Id} not found.");
            }

            return ApiResponse<WarehouseDetailDto>.Ok(warehouse, "Warehouse retrieved successfully.");

        }
    }
}
