using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Buildings.Commands.UpdateBuilding
{
    [Authorize(Roles = "Owner,Admin")]
    public record UpdateBuildingCommand(
        Guid Id,
        string Title,
        string? Description,
        string Purpose,
        decimal Price,
        decimal Area,
        string City,
        string District,
        string? Address,
        double? Latitude,
        double? Longitude,
        int? ParkingSpots,
        int? AgeInYears,
        string? FacingDirection,
        string? RegaLicenseNumber,
        string? DeedNumber,
        string? MunicipalityNumber,
        bool IsFeatured,
        bool IsPublished,

        // ── Building specific ─────────────────────────
        int? TotalFloors,
        int? UnitsCount,
        int? BasementFloors,
        bool HasElevator,
        bool HasParkingFloor,
        bool HasMosque,
        bool HasGuard,
        bool HasGenerator,
        bool HasCCTV,

        // ── Relations ─────────────────────────────────
        Guid? OwnerId,
        Guid? AgentId

    ) : IRequest<ApiResponse<bool>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
