using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandHandler
         : IRequestHandler<UpdateWarehouseCommand, ApiResponse<bool>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWarehouseCommandHandler(
            IWarehouseRepository warehouseRepository,
            IPropertyLookupRepository propertyLookupRepository,
            IOwnerRepository ownerRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _warehouseRepository = warehouseRepository;
            _propertyLookupRepository = propertyLookupRepository;
            _ownerRepository = ownerRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<bool>> Handle(UpdateWarehouseCommand request, CancellationToken ct)
        {
            var warehouse = await _warehouseRepository.GetByIdForUpdateAsync(request.Id, request.CompanyId, ct);
            if (warehouse is null)
                throw new NotFoundException("المستودع غير موجود");

            var dto = request.Warehouse;

            // Owner must exist in same company
            var owner = await _ownerRepository.GetByIdAsync(dto.OwnerId);
            if (owner is null || owner.CompanyId != request.CompanyId || owner.IsDeleted)
                throw new ValidationException("المالك غير موجود في نفس الشركة");

            // Agent must exist in same company
            var agent = await _userRepository.GetByIdAsync(dto.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId || agent.IsDeleted)
                throw new ValidationException("الوسيط/الموظف المسؤول غير موجود في نفس الشركة");

            // Parent property optional, but if provided must exist in same company
            if (dto.ParentPropertyId.HasValue)
            {
                // prevent self-parent
                if (dto.ParentPropertyId.Value == warehouse.Id)
                    throw new ValidationException("لا يمكن ربط المستودع بنفسه كعقار أب");

                var parentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                    dto.ParentPropertyId.Value,
                    request.CompanyId,
                    ct);

                if (!parentExists)
                    throw new ValidationException("العقار الأب غير موجود في نفس الشركة");
            }

            warehouse.ParentPropertyId = dto.ParentPropertyId;

            warehouse.Title = dto.Title;
            warehouse.Description = dto.Description;
            warehouse.Purpose = dto.Purpose;
            warehouse.PropertyStatus = dto.PropertyStatus;
            warehouse.Price = dto.Price;
            warehouse.Area = dto.Area;
            warehouse.City = dto.City;
            warehouse.District = dto.District;
            warehouse.Address = dto.Address;
            warehouse.Latitude = dto.Latitude;
            warehouse.Longitude = dto.Longitude;
            warehouse.ParkingSpots = dto.ParkingSpots;
            warehouse.AgeInYears = dto.AgeInYears;
            warehouse.FacingDirection = dto.FacingDirection;
            warehouse.FurnishedStatus = dto.FurnishedStatus;
            warehouse.RegaLicenseNumber = dto.RegaLicenseNumber;
            warehouse.DeedNumber = dto.DeedNumber;
            warehouse.MunicipalityNumber = dto.MunicipalityNumber;
            warehouse.IsFeatured = dto.IsFeatured;

            warehouse.OwnerId = dto.OwnerId;
            warehouse.AgentId = dto.AgentId;

            warehouse.CeilingHeight = dto.CeilingHeight;
            warehouse.LoadingDocks = dto.LoadingDocks;
            warehouse.ElectricityCapacity = dto.ElectricityCapacity;
            warehouse.OfficeSpace = dto.OfficeSpace;
            warehouse.SecurityRoom = dto.SecurityRoom;

            _warehouseRepository.Update(warehouse);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث المستودع بنجاح");
        }
    }
}
