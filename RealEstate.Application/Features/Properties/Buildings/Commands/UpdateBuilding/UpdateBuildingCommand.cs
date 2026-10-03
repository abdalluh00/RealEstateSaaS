using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Buildings.Commands.UpdateBuilding
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class UpdateBuildingCommand
        : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }
        public Guid CompanyId { get; private set; }

        // ── Base ──────────────────────────────────────────
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public PropertyPurpose Purpose { get; init; }
        public decimal Price { get; init; }
        public decimal Area { get; init; }
        public string City { get; init; } = string.Empty;
        public string District { get; init; } = string.Empty;
        public string? Address { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public int? ParkingSpots { get; init; }
        public int? AgeInYears { get; init; }
        public FacingDirection? FacingDirection { get; init; }
        public string? RegaLicenseNumber { get; init; }
        public string? DeedNumber { get; init; }
        public string? MunicipalityNumber { get; init; }
        public Guid? OwnerId { get; init; }
        public Guid? AgentId { get; init; }
        public bool IsFeatured { get; init; }
        public bool IsPublished { get; init; }

        // ── Building specific ─────────────────────────────
        public int? TotalFloors { get; init; }
        public int? UnitsCount { get; init; }
        public int? BasementFloors { get; init; }
        public bool HasElevator { get; init; }
        public bool HasParkingFloor { get; init; }
        public bool HasMosque { get; init; }
        public bool HasGuard { get; init; }
        public bool HasGenerator { get; init; }
        public bool HasCCTV { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}