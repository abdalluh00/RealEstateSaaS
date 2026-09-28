using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Features.Properties.Buildings.Commands.UpdateBuilding;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Building.Commands.UpdateBuilding
{
    public class UpdateBuildingHandler : IRequestHandler<UpdateBuildingCommand, ApiResponse<bool>>
    {
        private readonly IBuildingRepository _buildingRepo;
        private readonly IUnitOfWork _uow;

        public UpdateBuildingHandler(
            IBuildingRepository buildingRepo,
            IUnitOfWork uow)
        {
            _buildingRepo = buildingRepo;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateBuildingCommand request,
            CancellationToken ct)
        {
            // ── Fetch tracked entity ──────────────────────
            var building = await _buildingRepo
                .Query()
                .FirstOrDefaultAsync(b => b.Id == request.Id
                                       && b.CompanyId == request.CompanyId, ct)
                ?? throw new NotFoundException("المبنى غير موجود");

            // ── Update base fields ────────────────────────
            building.Title = request.Title;
            building.Description = request.Description;
            building.Purpose = Enum.Parse<PropertyPurpose>(request.Purpose);
            building.Price = request.Price;
            building.Area = request.Area;
            building.City = request.City;
            building.District = request.District;
            building.Address = request.Address;
            building.Latitude = request.Latitude;
            building.Longitude = request.Longitude;
            building.ParkingSpots = request.ParkingSpots;
            building.AgeInYears = request.AgeInYears;
            building.RegaLicenseNumber = request.RegaLicenseNumber;
            building.DeedNumber = request.DeedNumber;
            building.MunicipalityNumber = request.MunicipalityNumber;
            building.IsFeatured = request.IsFeatured;
            building.IsPublished = request.IsPublished;
            building.OwnerId = request.OwnerId;
            building.AgentId = request.AgentId;

            // ── Update building specific fields ───────────
            building.TotalFloors = request.TotalFloors;
            building.UnitsCount = request.UnitsCount;
            building.BasementFloors = request.BasementFloors;
            building.HasElevator = request.HasElevator;
            building.HasParkingFloor = request.HasParkingFloor;
            building.HasMosque = request.HasMosque;
            building.HasGuard = request.HasGuard;
            building.HasGenerator = request.HasGenerator;
            building.HasCCTV = request.HasCCTV;

            _buildingRepo.Update(building);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true);
        }
    }
}