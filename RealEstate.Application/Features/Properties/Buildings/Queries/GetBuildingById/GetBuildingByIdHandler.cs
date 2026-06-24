using MediatR;
using RealEstate.Application.Features.Properties.Buildings.Dtos;
using RealEstate.Domain.Interfaces.Properties;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Queries.GetBuildingById
{
    public class GetBuildingByIdHandler : IRequestHandler<GetBuildingByIdQuery, ApiResponse<BuildingDetailsDto>>
    {
        private readonly IBuildingRepository _buildingRepository;

        public GetBuildingByIdHandler(IBuildingRepository buildingRepository)
        {
            _buildingRepository = buildingRepository;
        }

        public async Task<ApiResponse<BuildingDetailsDto>> Handle(GetBuildingByIdQuery request, CancellationToken ct)
        {
            var entity = await _buildingRepository.GetByIdWithDetailsAsync(request.Id, request.CompanyId);
            if (entity is null)
                throw new NotFoundException("المبنى غير موجود");

            var dto = new BuildingDetailsDto
            {
                Id = entity.Id,
                PropertyCode = entity.PropertyCode,
                ParentPropertyId = entity.ParentPropertyId,

                Title = entity.Title,
                Description = entity.Description,

                Purpose = entity.Purpose.ToString(),
                PropertyStatus = entity.PropertyStatus.ToString(),

                Price = entity.Price,
                Area = entity.Area,

                City = entity.City,
                District = entity.District,
                Address = entity.Address,
                Latitude = entity.Latitude,
                Longitude = entity.Longitude,

                ParkingSpots = entity.ParkingSpots,
                AgeInYears = entity.AgeInYears,
                FacingDirection = entity.FacingDirection,
                FurnishedStatus = entity.FurnishedStatus.ToString(),

                RegaLicenseNumber = entity.RegaLicenseNumber,
                DeedNumber = entity.DeedNumber,
                MunicipalityNumber = entity.MunicipalityNumber,

                IsFeatured = entity.IsFeatured,

                OwnerId = entity.OwnerId ?? Guid.Empty,
                AgentId = entity.AgentId ?? Guid.Empty,

                TotalFloors = entity.TotalFloors,
                UnitsCount = entity.UnitsCount,
                Elevator = entity.Elevator,
                ParkingFloor = entity.ParkingFloor
            };

            return ApiResponse<BuildingDetailsDto>.Ok(dto);
        }
    }
}
