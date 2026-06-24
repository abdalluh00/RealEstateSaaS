using MediatR;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Domain.ReadModels;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Queries.GetWarehouseById
{
    public class GetWarehouseByIdQueryHandler
        : IRequestHandler<GetWarehouseByIdQuery, ApiResponse<WarehouseDetailsDto>>
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public GetWarehouseByIdQueryHandler(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public async Task<ApiResponse<WarehouseDetailsDto>> Handle(GetWarehouseByIdQuery request, CancellationToken ct)
        {
            var warehouse = await _warehouseRepository.GetDetailsByIdAsync(request.Id, request.CompanyId, ct);
            if (warehouse is null)
                throw new NotFoundException("المستودع غير موجود");

            return ApiResponse<WarehouseDetailsDto>.Ok(warehouse);
        }
    }
}
