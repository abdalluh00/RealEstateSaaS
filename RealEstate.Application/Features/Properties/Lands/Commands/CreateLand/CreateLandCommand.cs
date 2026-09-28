using MediatR;
using RealEstate.Application.Common.Behaviors;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Authorization;
using RealEstate.Shared.Common;

namespace RealEstate.Application.Features.Lands.Commands.CreateLand
{
    [Authorize(Roles = $"{Roles.Admin}")]
    public sealed class CreateLandCommand
        : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
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
        public int? AgeInYears { get; init; }
        public FacingDirection FacingDirection { get; init; }
        public string? RegaLicenseNumber { get; init; }
        public string? DeedNumber { get; init; }
        public string? MunicipalityNumber { get; init; }
        public Guid? OwnerId { get; init; }
        public Guid? AgentId { get; init; }

        // ── Land specific ─────────────────────────────────
        public decimal? StreetWidth { get; init; }
        public int? NumberOfStreets { get; init; }
        public ZoningType ZoningType { get; init; }
        public LandShape LandShape { get; init; }
        public bool IsCornerLand { get; init; }
        public bool IsWalled { get; init; }
        public bool HasElectricity { get; init; }
        public bool HasWater { get; init; }
        public bool HasSewer { get; init; }

        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}