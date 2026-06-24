using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Apartments.DTOs
{
    public class ApartmentDetailsDto
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;

        // Shared Property fields
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

        public Guid? ParentPropertyId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid AgentId { get; set; }

        // Apartment fields
        public string UnitNumber { get; set; } = string.Empty;
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int FloorNumber { get; set; }
        public int? LivingRooms { get; set; }

        public bool HasMaidRoom { get; set; }
        public bool HasElevator { get; set; }
        public bool HasCentralAc { get; set; }
        public bool HasBalcony { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}