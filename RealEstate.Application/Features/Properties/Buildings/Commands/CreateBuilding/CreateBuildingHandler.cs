using MediatR;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Commands.CreateBuilding
{
    public class CreateBuildingHandler : IRequestHandler<CreateBuildingCommand, ApiResponse<Guid>>
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Property> _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBuildingHandler(
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

        public async Task<ApiResponse<Guid>> Handle(CreateBuildingCommand request, CancellationToken ct)
        {
            // Owner validation
            var owner = await _ownerRepository.GetByIdAsync(request.OwnerId);
            if (owner is null || owner.CompanyId != request.CompanyId)
                throw new ValidationException("المالك غير موجود أو لا يتبع لنفس الشركة");

            // Agent validation
            var agent = await _userRepository.GetByIdAsync(request.AgentId);
            if (agent is null || agent.CompanyId != request.CompanyId)
                throw new ValidationException("الوسيط/الموظف المسؤول غير موجود أو لا يتبع لنفس الشركة");

            // Parent validation if exists
            if (request.ParentPropertyId.HasValue)
            {
                var parent = await _propertyRepository.GetByIdAsync(request.ParentPropertyId.Value);
                if (parent is null || parent.CompanyId != request.CompanyId)
                    throw new ValidationException("العقار الأب غير موجود أو لا يتبع لنفس الشركة");
            }

            var entity = new BuildingProperty
            {
                PropertyCode = GeneratePropertyCode(),
                ParentPropertyId = request.ParentPropertyId,

                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                Purpose = request.Purpose,
                PropertyStatus = request.PropertyStatus,

                Price = request.Price,
                Area = request.Area,

                City = request.City.Trim(),
                District = request.District.Trim(),
                Address = request.Address?.Trim(),
                Latitude = request.Latitude,
                Longitude = request.Longitude,

                ParkingSpots = request.ParkingSpots,
                AgeInYears = request.AgeInYears,
                FacingDirection = request.FacingDirection?.Trim(),
                FurnishedStatus = request.FurnishedStatus,

                RegaLicenseNumber = request.RegaLicenseNumber?.Trim(),
                DeedNumber = request.DeedNumber?.Trim(),
                MunicipalityNumber = request.MunicipalityNumber?.Trim(),

                IsFeatured = request.IsFeatured,

                CompanyId = request.CompanyId,
                OwnerId = request.OwnerId,
                AgentId = request.AgentId,

                TotalFloors = request.TotalFloors,
                UnitsCount = request.UnitsCount,
                Elevator = request.Elevator,
                ParkingFloor = request.ParkingFloor
            };

            await _buildingRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(entity.Id, "تم إنشاء المبنى بنجاح");
        }

        private static string GeneratePropertyCode()
            => $"BLD-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
