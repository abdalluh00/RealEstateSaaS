using MediatR;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Buildings.Commands.UpdateBuilding
{
    public sealed class UpdateBuildingCommandHandler
        : IRequestHandler<UpdateBuildingCommand, ApiResponse<bool>>
    {
        private readonly IBuildingRepository _buildings;
        private readonly IUnitOfWork _uow;

        public UpdateBuildingCommandHandler(
            IBuildingRepository buildings,
            IUnitOfWork uow)
        {
            _buildings = buildings;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            UpdateBuildingCommand cmd,
            CancellationToken ct)
        {
            var building = await _buildings.Query()
                .FirstOrDefaultAsync(x => x.Id == cmd.Id
                                       && x.CompanyId == cmd.CompanyId, ct);

            if (building is null)
                throw new NotFoundException("العمارة", cmd.Id);

            building.Title = cmd.Title;
            building.Description = cmd.Description;
            building.Purpose = cmd.Purpose;
            building.Price = cmd.Price;
            building.Area = cmd.Area;
            building.City = cmd.City;
            building.District = cmd.District;
            building.Address = cmd.Address;
            building.Latitude = cmd.Latitude;
            building.Longitude = cmd.Longitude;
            building.ParkingSpots = cmd.ParkingSpots;
            building.AgeInYears = cmd.AgeInYears;
            building.FacingDirection = (Domain.Common.Enums.FacingDirection)cmd.FacingDirection!;
            building.RegaLicenseNumber = cmd.RegaLicenseNumber;
            building.DeedNumber = cmd.DeedNumber;
            building.MunicipalityNumber = cmd.MunicipalityNumber;
            building.OwnerId = cmd.OwnerId;
            building.AgentId = cmd.AgentId;
            building.IsFeatured = cmd.IsFeatured;
            building.IsPublished = cmd.IsPublished;
            building.TotalFloors = cmd.TotalFloors;
            building.UnitsCount = cmd.UnitsCount;
            building.BasementFloors = cmd.BasementFloors;
            building.HasElevator = cmd.HasElevator;
            building.HasParkingFloor = cmd.HasParkingFloor;
            building.HasMosque = cmd.HasMosque;
            building.HasGuard = cmd.HasGuard;
            building.HasGenerator = cmd.HasGenerator;
            building.HasCCTV = cmd.HasCCTV;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تحديث العمارة بنجاح");
        }
    }
}