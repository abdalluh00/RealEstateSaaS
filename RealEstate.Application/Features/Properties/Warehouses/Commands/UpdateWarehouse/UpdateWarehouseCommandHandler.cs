using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Properties.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandHandler
         : IRequestHandler<UpdateWarehouseCommand, ApiResponse<bool>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
          
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
           
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateWarehouseCommand request, CancellationToken ct)
        {
           

           var  warehouse = await _warehouseRepository.Query().FirstOrDefaultAsync(x=> x.Id == request.Id && request.CompanyId == x.CompanyId , ct);
           
            if(warehouse == null)
            {
                throw new NotFoundException($"المستودع بالمعرف {request.Id} غير موجود");
            }
            warehouse.Title = request.Title ;
            warehouse.Description = request.Description;
            warehouse.Purpose = request.Purpose;
            warehouse.PropertyStatus = request.PropertyStatus;
            warehouse.Price = request.Price;
            warehouse.Area = request.Area;
            warehouse.City = request.City;
            warehouse.District = request.District;
            warehouse.Address = request.Address;
            warehouse.Latitude = request.Latitude;
            warehouse.Longitude = request.Longitude;
            warehouse.ParkingSpots = request.ParkingSpots;
            warehouse.AgeInYears = request.AgeInYears;
            warehouse.FacingDirection = request.FacingDirection;
            warehouse.RegaLicenseNumber = request.RegaLicenseNumber;
            warehouse.DeedNumber = request.DeedNumber;
            warehouse.MunicipalityNumber = request.MunicipalityNumber;
            warehouse.IsFeatured = request.IsFeatured;

            warehouse.OwnerId = request.OwnerId;
            warehouse.AgentId = request.AgentId;

            warehouse.CeilingHeight = request.CeilingHeight;
            warehouse.LoadingDocks = request.LoadingDocks;
            warehouse.ElectricityCapacity = (ElectricityCapacity) request.ElectricityCapacity!;
          

            _warehouseRepository.Update(warehouse);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث المستودع بنجاح");
        }
    }
}
