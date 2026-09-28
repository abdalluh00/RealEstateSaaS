using MediatR;
using RealEstate.Application.Features.Properties.Buildings.Commands.CreateBuilding;
using RealEstate.Application.Interfaces;
using RealEstate.Application.Interfaces.Properties;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Properties.Building.Commands.CreateBuilding
{
    public class CreateBuildingHandler : IRequestHandler<CreateBuildingCommand, ApiResponse<Guid>>
    {
        private readonly IBuildingRepository _buildingRepo;
        private readonly IPropertyRepository _propertyRepo;
        private readonly IUnitOfWork _uow;
        private readonly IPropertyCodeGenerator _codeGenerator;

        public CreateBuildingHandler(
            IBuildingRepository buildingRepo,
            IPropertyRepository propertyRepo,
            IUnitOfWork uow,
            IPropertyCodeGenerator codeGenerator)
        {
            _buildingRepo = buildingRepo;
            _propertyRepo = propertyRepo;
            _uow = uow;
            _codeGenerator = codeGenerator;
        }

        public async Task<ApiResponse<Guid>> Handle(
            CreateBuildingCommand request,
            CancellationToken ct)
        {
            // ── Validate Owner exists in company ──────────
            if (request.OwnerId.HasValue)
            {
                var ownerExists = await _propertyRepo.IsOwnerLinkedToAnyPropertyAsync(
                    request.OwnerId.Value, request.CompanyId, ct);
                // Note: use IOwnerRepository.ExistsAsync instead when built
            }

            // ── Generate unique property code ─────────────
            var code = await _codeGenerator.GenerateAsync(
                request.CompanyId, "BLD", ct);

            // ── Build entity ──────────────────────────────
            var building = new BuildingProperty
            {
                PropertyCode = code,
                Title = request.Title,
                Description = request.Description,
                Purpose = Enum.Parse<PropertyPurpose>(request.Purpose),
                PropertyStatus = PropertyStatus.Available,
                Price = request.Price,
                Area = request.Area,
                City = request.City,
                District = request.District,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                ParkingSpots = request.ParkingSpots,
                AgeInYears = request.AgeInYears,
                RegaLicenseNumber = request.RegaLicenseNumber,
                DeedNumber = request.DeedNumber,
                MunicipalityNumber = request.MunicipalityNumber,
                IsFeatured = request.IsFeatured,
                IsPublished = request.IsPublished,
                OwnerId = request.OwnerId,
                AgentId = request.AgentId,
                CompanyId = request.CompanyId,

                // ── Building specific ─────────────────────
                TotalFloors = request.TotalFloors,
                UnitsCount = request.UnitsCount,
                BasementFloors = request.BasementFloors,
                HasElevator = request.HasElevator,
                HasParkingFloor = request.HasParkingFloor,
                HasMosque = request.HasMosque,
                HasGuard = request.HasGuard,
                HasGenerator = request.HasGenerator,
                HasCCTV = request.HasCCTV,

                // ── Building never has parent ─────────────
                ParentPropertyId = null,
                UnitNumber = null
            };

            _buildingRepo.Add(building);
            await _uow.SaveChangesAsync(ct);

            return ApiResponse<Guid>.Ok(building.Id);
        }
    }
}