using MediatR;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Commands.CreateBuilding
{
    public sealed class CreateBuildingCommandHandler
        : IRequestHandler<CreateBuildingCommand, ApiResponse<Guid>>
    {
        private readonly IBuildingRepository _buildings;
        private readonly IUnitOfWork _uow;
        private readonly IPropertyCodeGenerator _codeGenerator;

        public CreateBuildingCommandHandler(
            IBuildingRepository buildings,
            IUnitOfWork uow,
            IPropertyCodeGenerator codeGenerator)
        {
            _buildings = buildings;
            _uow = uow;
            _codeGenerator = codeGenerator;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateBuildingCommand cmd,
            CancellationToken ct)
        {
            var code = await _codeGenerator.GenerateAsync(cmd.CompanyId,"BUILD", ct);

            var building = new BuildingProperty
            {
                PropertyCode = code,
                Type = PropertyType.Building,
                Title = cmd.Title,
                Description = cmd.Description,
                Purpose = cmd.Purpose,
                PropertyStatus = PropertyStatus.Available,
                Price = cmd.Price,
                Area = cmd.Area,
                City = cmd.City,
                District = cmd.District,
                Address = cmd.Address,
                Latitude = cmd.Latitude,
                Longitude = cmd.Longitude,
                ParkingSpots = cmd.ParkingSpots,
                AgeInYears = cmd.AgeInYears,
                FacingDirection = (FacingDirection)cmd.FacingDirection!,
                RegaLicenseNumber = cmd.RegaLicenseNumber,
                DeedNumber = cmd.DeedNumber,
                MunicipalityNumber = cmd.MunicipalityNumber,
                CompanyId = cmd.CompanyId,
                OwnerId = cmd.OwnerId,
                AgentId = cmd.AgentId,
                TotalFloors = cmd.TotalFloors,
                UnitsCount = cmd.UnitsCount,
                BasementFloors = cmd.BasementFloors,
                HasElevator = cmd.HasElevator,
                HasParkingFloor = cmd.HasParkingFloor,
                HasMosque = cmd.HasMosque,
                HasGuard = cmd.HasGuard,
                HasGenerator = cmd.HasGenerator,
                HasCCTV = cmd.HasCCTV
            };

            _buildings.Add(building);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(building.Id, "تم إنشاء العمارة بنجاح");
        }
    }
}