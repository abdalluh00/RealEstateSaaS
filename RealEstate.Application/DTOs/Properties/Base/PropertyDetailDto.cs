using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Base
{
    public record PropertyDetailDto : PropertyListDto
    {
        public string? Description { get; init; }
        public string? Address { get; init; }
        public double? Latitude { get; init; }
        public double? Longitude { get; init; }
        public int? ParkingSpots { get; init; }
        public int? AgeInYears { get; init; }
        public string? FacingDirection { get; init; }
        public string? RegaLicenseNumber { get; init; }
        public string? DeedNumber { get; init; }
        public string? MunicipalityNumber { get; init; }
        public bool IsPublished { get; init; }

        // ── Owner ─────────────────────────────────────────
        public Guid? OwnerId { get; init; }
        public string? OwnerName { get; init; }   // ← add here, not in list
        public string? OwnerPhone { get; init; }

        // ── Agent ─────────────────────────────────────────
        public Guid? AgentId { get; init; }
        public string? AgentName { get; init; }   // ← add here, not in list
        public string? AgentPhone { get; init; }

        public DateTime? UpdatedAt { get; init; }
        public IEnumerable<PropertyMediaDto> Media { get; init; } = [];
        public IEnumerable<PropertyDocumentDto> Documents { get; init; } = [];
    }
}
