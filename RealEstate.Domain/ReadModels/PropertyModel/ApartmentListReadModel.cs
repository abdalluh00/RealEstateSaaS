using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.ReadModels.PropertyModel
{
    public sealed record ApartmentListReadModel
    {
        // ── Base fields ───────────────────────────────────
        public Guid Id { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public PropertyPurpose Purpose { get; init; }
        public PropertyStatus PropertyStatus { get; init; }
        public decimal Price { get; init; }
        public decimal Area { get; init; }
        public string City { get; init; } = string.Empty;
        public string District { get; init; } = string.Empty;
        public bool IsFeatured { get; init; }
        public bool IsPublished { get; init; }
        public DateTime CreatedAt { get; init; }

        // ── Apartment-specific ────────────────────────────
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public int? FloorNumber { get; init; }
        public FurnishedStatus? FurnishedStatus { get; init; }
        public bool HasElevator { get; init; }
        public bool HasBalcony { get; init; }
    }
}
