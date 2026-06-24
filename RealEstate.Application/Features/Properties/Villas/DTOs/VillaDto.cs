using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.DTOs
{
    public class VillaDto
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;

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

        public Guid CompanyId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? AgentId { get; set; }

        // Villa fields
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Floors { get; set; }
        public bool MaidRoom { get; set; }
        public bool DriverRoom { get; set; }
        public bool Pool { get; set; }
        public decimal? GardenArea { get; set; }
    }
}
