using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.ReadModels.PropertyModel
{
    public sealed record ApartmentDetailReadModel
    {
        // ── Base fields ───────────────────────────────────
        public Guid Id { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
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
        public bool IsFeatured { get; init; }
        public bool IsPublished { get; init; }

        // ── Saudi Legal ───────────────────────────────────
        public string? DeedNumber { get; init; }
        public string? RegaLicenseNumber { get; init; }
        public string? MunicipalityNumber { get; init; }

        // ── Apartment-specific ────────────────────────────
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

        // ── Relations ─────────────────────────────────────
        public Guid? OwnerId { get; init; }
        public string? OwnerName { get; init; }
        public Guid? AgentId { get; init; }
        public string? AgentName { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
