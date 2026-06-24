using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Dtos
{
    public sealed class LandDetailsDto
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;
        public Guid? ParentPropertyId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public string Purpose { get; set; } = string.Empty;
        public string PropertyStatus { get; set; } = string.Empty;

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
        public string FurnishedStatus { get; set; } = string.Empty;

        public string? RegaLicenseNumber { get; set; }
        public string? DeedNumber { get; set; }
        public string? MunicipalityNumber { get; set; }

        public bool IsFeatured { get; set; }

        public Guid OwnerId { get; set; }
        public Guid AgentId { get; set; }

        public decimal? StreetWidth { get; set; }
        public string? ZoningType { get; set; }
        public bool CornerLand { get; set; }
        public int? NumberOfStreets { get; set; }
    }
}
