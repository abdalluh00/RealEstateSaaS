
using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class CreateApartmentCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid CompanyId { get; private set; }

        // ── Base fields ───────────────────────────────────
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
        public Guid? ParentPropertyId { get; init; }
        public string? UnitNumber { get; init; }

        // ── Apartment specific ────────────────────────────
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public int? LivingRooms { get; init; }
        public int? FloorNumber { get; init; }
        public bool HasMaidRoom { get; init; }
        public bool HasElevator { get; init; }
        public bool HasCentralAC { get; init; }
        public bool HasBalcony { get; init; }
        public bool HasStorage { get; init; }
        public FurnishedStatus? FurnishedStatus { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
