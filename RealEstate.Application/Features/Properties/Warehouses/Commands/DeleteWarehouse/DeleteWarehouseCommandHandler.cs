using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.DeleteWarehouse
{
    public class DeleteWarehouseCommandHandler
         : IRequestHandler<DeleteWarehouseCommand, ApiResponse<bool>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteWarehouseCommand request, CancellationToken ct)
        {
            var warehouse = await _warehouseRepository.Query().FirstOrDefaultAsync(x=> x.Id == request.Id &&
            x.CompanyId == request.CompanyId, ct);
            if (warehouse is null)
                throw new NotFoundException("المستودع غير موجود");

           

            _warehouseRepository.SoftDelete(warehouse);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم حذف المستودع بنجاح");
        }
    }
}
