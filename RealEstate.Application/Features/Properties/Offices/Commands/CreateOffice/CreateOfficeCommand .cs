using MediatR;
using Microsoft.AspNetCore.Authorization;
using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Offices.Commands.CreateOffice
{
    [Authorize(Roles = "Owner,Admin,Agent")]
    public record CreateOfficeCommand : IRequest<ApiResponse<Guid>>, IAutoTenantRequest
    {
        // Common Property
        public string PropertyCode { get; init; } = string.Empty;
        public Guid? ParentPropertyId { get; init; }

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
        public FacingDirection FacingDirection { get; init; }
        public FurnishedStatus FurnishedStatus { get; init; }

        public string? RegaLicenseNumber { get; init; }
        public string? DeedNumber { get; init; }
        public string? MunicipalityNumber { get; init; }

        public bool IsFeatured { get; init; }

        public Guid? OwnerId { get; init; }
        public Guid? AgentId { get; init; }

        // Office-specific
        public string? UnitNumber { get; init; }
        public int Floor { get; init; }
        public int Bathrooms { get; init; }
        public int OfficesCount { get; init; }
        public int? MeetingRooms { get; init; }
        public bool Elevator { get; init; }
        public bool CentralAC { get; init; }
        public bool ReceptionArea { get; init; }

        public Guid CompanyId { get; private set; }
        public void SetCompanyId(Guid companyId) => CompanyId = companyId;
    }
}
