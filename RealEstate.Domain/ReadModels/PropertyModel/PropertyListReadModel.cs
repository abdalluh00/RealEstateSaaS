using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.ReadModels.PropertyModel
{
    // Used for mixed list across all property types
    // Only base Property columns — no subtype join
    public sealed record PropertyListReadModel
    {
        public Guid Id { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;      // discriminator from EF
        public PropertyPurpose Purpose { get; init; }
        public PropertyStatus PropertyStatus { get; init; }
        public string? UnitNumber { get; init; }

        public decimal Price { get; init; }
        public decimal Area { get; init; }
        public string City { get; init; } = string.Empty;
        public string District { get; init; } = string.Empty;
        public bool IsFeatured { get; init; }
        public bool IsPublished { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
