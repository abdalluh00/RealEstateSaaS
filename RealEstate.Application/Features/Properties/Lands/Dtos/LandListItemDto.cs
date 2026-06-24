using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Lands.Dtos
{
    public sealed class LandListItemDto
    {
        public Guid Id { get; set; }
        public string PropertyCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal Area { get; set; }
        public string City { get; set; } = string.Empty;
        public string District { get; set; } = string.Empty;
        public string PropertyStatus { get; set; } = string.Empty;

        public decimal? StreetWidth { get; set; }
        public string? ZoningType { get; set; }
        public bool CornerLand { get; set; }
        public int? NumberOfStreets { get; set; }

        public Guid OwnerId { get; set; }
        public Guid AgentId { get; set; }
    }
}
