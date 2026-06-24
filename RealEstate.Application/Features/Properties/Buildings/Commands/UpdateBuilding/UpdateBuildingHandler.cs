using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Commands.UpdateBuilding
{
    public class UpdateBuildingHandler : IRequestHandler<UpdateBuildingCommand, ApiResponse<Guid>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Property> _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBuildingHandler(
            IBuildingRepository buildingRepository,
            IOwnerRepository ownerRepository,
            IUserRepository userRepository,
            IGenericRepository<Property> propertyRepository,
            IUnitOfWork unitOfWork)
        {
            _buildingRepository = buildingRepository;
            _ownerRepository = ownerRepository;
            _userRepository = userRepository;
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<Guid>> Handle(UpdateBuildingCommand request, CancellationToken ct)
        {
            var entity = await _buildingRepository.GetByIdAsync(request.Id, request.CompanyId);
            if (entity is null)
                throw new NotFoundException("المبنى غير موجود");

            var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
            if (owner is null || owner.CompanyId != request.CompanyId)
                throw new ValidationException("المالك غير موجود أو لا يتبع لنفس الشركة");

            var agent = await _userRepository.GetByIdAsync(request.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId)
                throw new ValidationException("الوسيط/الموظف المسؤول غير موجود أو لا يتبع لنفس الشركة");

            if (request.ParentPropertyId.HasValue)
            {
                if (request.ParentPropertyId.Value == request.Id)
                    throw new ValidationException("لا يمكن ربط المبنى بنفسه كعقار أب");

                var parent = await _propertyRepository.GetByIdAsync(request.ParentPropertyId.Value);
                if (parent is null || parent.CompanyId != request.CompanyId)
                    throw new ValidationException("العقار الأب غير موجود أو لا يتبع لنفس الشركة");
            }

            entity.ParentPropertyId = request.ParentPropertyId;

            entity.Title = request.Title.Trim();
            entity.Description = request.Description?.Trim();
            entity.Purpose = request.Purpose;
            entity.PropertyStatus = request.PropertyStatus;

            entity.Price = request.Price;
            entity.Area = request.Area;

            entity.City = request.City.Trim();
            entity.District = request.District.Trim();
            entity.Address = request.Address?.Trim();
            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;

            entity.ParkingSpots = request.ParkingSpots;
            entity.AgeInYears = request.AgeInYears;
            entity.FacingDirection = request.FacingDirection?.Trim();
            entity.FurnishedStatus = request.FurnishedStatus;

            entity.RegaLicenseNumber = request.RegaLicenseNumber?.Trim();
            entity.DeedNumber = request.DeedNumber?.Trim();
            entity.MunicipalityNumber = request.MunicipalityNumber?.Trim();

            entity.IsFeatured = request.IsFeatured;

            entity.OwnerId = request.OwnerId;
            entity.AgentId = request.AgentId;

            entity.TotalFloors = request.TotalFloors;
            entity.UnitsCount = request.UnitsCount;
            entity.Elevator = request.Elevator;
            entity.ParkingFloor = request.ParkingFloor;

            _buildingRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(entity.Id, "تم تحديث المبنى بنجاح");
        }
    }
}
