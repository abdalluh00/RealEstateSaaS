using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.CreateWarehouse
{
    public class CreateWarehouseCommandHandler
       : IRequestHandler<CreateWarehouseCommand, ApiResponse<Guid>>
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IPropertyLookupRepository _propertyLookupRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWarehouseCommandHandler(
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

        public async Task<ApiResponse<Guid>> Handle(CreateWarehouseCommand request, CancellationToken ct)
        {
            var dto = request.Warehouse;

            // Owner must exist in same company
            var owner = await _ownerRepository.GetByIdAsync(dto.OwnerId);
            if (owner is null || owner.CompanyId != request.CompanyId || owner.IsDeleted)
                throw new ValidationException("المالك غير موجود في نفس الشركة");

            // Agent must exist in same company
            var agent = await _userRepository.GetByIdAsync(dto.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId || agent.IsDeleted)
                throw new ValidationException("الوسيط/الموظف المسؤول غير موجود في نفس الشركة");

            // Parent property optional, but if provided it must exist in same company
            if (dto.ParentPropertyId.HasValue)
            {
                var parentExists = await _propertyLookupRepository.ExistsInCompanyAsync(
                    dto.ParentPropertyId.Value,
                    request.CompanyId,
                    ct);

                if (!parentExists)
                    throw new ValidationException("العقار الأب غير موجود في نفس الشركة");
            }

            var warehouse = new WarehouseProperty
            {
                CompanyId = request.CompanyId,

                ParentPropertyId = dto.ParentPropertyId,

                Title = dto.Title,
                Description = dto.Description,
                Purpose = dto.Purpose,
                PropertyStatus = dto.PropertyStatus,
                Price = dto.Price,
                Area = dto.Area,
                City = dto.City,
                District = dto.District,
                Address = dto.Address,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                ParkingSpots = dto.ParkingSpots,
                AgeInYears = dto.AgeInYears,
                FacingDirection = dto.FacingDirection,
                FurnishedStatus = dto.FurnishedStatus,
                RegaLicenseNumber = dto.RegaLicenseNumber,
                DeedNumber = dto.DeedNumber,
                MunicipalityNumber = dto.MunicipalityNumber,
                IsFeatured = dto.IsFeatured,

                OwnerId = dto.OwnerId,
                AgentId = dto.AgentId,

                CeilingHeight = dto.CeilingHeight,
                LoadingDocks = dto.LoadingDocks,
                ElectricityCapacity = dto.ElectricityCapacity,
                OfficeSpace = dto.OfficeSpace,
                SecurityRoom = dto.SecurityRoom
            };

            await _warehouseRepository.AddAsync(warehouse);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(warehouse.Id, "تم إنشاء المستودع بنجاح");
        }
    }
}
