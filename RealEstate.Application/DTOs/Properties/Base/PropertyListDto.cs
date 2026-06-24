using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Base
{
    public record PropertyListDto
    {
        public Guid Id { get; init; }
        public string PropertyCode { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Purpose { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public decimal Area { get; init; }
        public string City { get; init; } = string.Empty;
        public string District { get; init; } = string.Empty;
        public string? UnitNumber { get; init; }
        public string? CoverImageUrl { get; init; }
        public string? OwnerName { get; init; }
        public string? AgentName { get; init; }
        public bool IsFeatured { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
