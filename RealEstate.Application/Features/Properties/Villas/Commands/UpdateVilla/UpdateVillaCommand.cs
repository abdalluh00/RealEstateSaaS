using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.UpdateVilla
{
    [Authorize(Roles = "Owner,Admin")]
    public record UpdateVillaCommand : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        public Guid Id { get; init; }

        // Base Property
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }

        public PropertyPurpose Purpose { get; init; }
        public PropertyStatus PropertyStatus { get; init; }

        public decimal Price { get; init; }
        public decimal Area { get; init; }

        public string City { get; init; } = string.Empty;
        public string District { get; init; } = string.Empty;
        public string? Address { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }

        public int? ParkingSpots { get; init; }
        public int? AgeInYears { get; init; }
        public string? FacingDirection { get; init; }
        public FurnishedStatus FurnishedStatus { get; init; }

        public string? RegaLicenseNumber { get; init; }
        public string? DeedNumber { get; init; }
        public string? MunicipalityNumber { get; init; }

        public bool IsFeatured { get; init; }

        public Guid? ParentPropertyId { get; init; }

        public Guid OwnerId { get; init; }
        public Guid AgentId { get; init; }

        // Villa fields
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public int Floors { get; init; }
        public bool MaidRoom { get; init; }
        public bool DriverRoom { get; init; }
        public bool Pool { get; init; }
        public decimal? GardenArea { get; init; }

        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
