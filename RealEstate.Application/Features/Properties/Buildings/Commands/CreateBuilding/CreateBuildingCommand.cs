using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.Properties.Buildings.Commands.CreateBuilding
{
    [Authorize(Roles = "Owner,Admin")]
    public record CreateBuildingCommand(
        string Title,
        string? Description,
        PropertyPurpose Purpose,
        PropertyStatus PropertyStatus,
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
        FurnishedStatus FurnishedStatus,
        string? RegaLicenseNumber,
        string? DeedNumber,
        string? MunicipalityNumber,
        bool IsFeatured,
        Guid OwnerId,
        Guid AgentId,
        Guid? ParentPropertyId,
        int? TotalFloors,
        int? UnitsCount,
        bool Elevator,
        bool ParkingFloor
    ) : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
