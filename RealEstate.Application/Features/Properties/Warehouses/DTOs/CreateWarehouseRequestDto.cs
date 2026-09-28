using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.DTOs
{
    public class CreateWarehouseRequestDto
    {
        // Shared property fields
        public Guid? ParentPropertyId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public PropertyPurpose Purpose { get; set; }
        public PropertyStatus PropertyStatus { get; set; }

        public decimal Price { get; set; }
        public decimal Area { get; set; }

        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int? ParkingSpots { get; set; }
        public int? AgeInYears { get; set; }
        public string? FacingDirection { get; set; }

        public FurnishedStatus FurnishedStatus { get; set; }

        public string? RegaLicenseNumber { get; set; }
        public string? DeedNumber { get; set; }
        public string? MunicipalityNumber { get; set; }

        public bool IsFeatured { get; set; }

        public Guid OwnerId { get; set; }
        public Guid AgentId { get; set; }

        // Warehouse specific
        public decimal? CeilingHeight { get; init; }
        public int? LoadingDocks { get; init; }
        public int? GateCount { get; init; }
        public ElectricityCapacity? ElectricityCapacity { get; init; }
        public bool HasOfficeSpace { get; init; }
        public bool HasSecurityRoom { get; init; }
        public bool HasCCTV { get; init; }
        public bool HasFireSystem { get; init; }
        public bool HasColdStorage { get; init; }
        public bool HasMosanada { get; init; }
        public bool IsFenced { get; init; }
        public bool HasTruckAccess { get; init; }
    }
}
