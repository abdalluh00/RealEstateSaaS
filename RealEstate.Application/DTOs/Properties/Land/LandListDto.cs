using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Land
{
    public record LandListDto : PropertyListDto
    {
        public decimal? StreetWidth { get; init; }
        public string? ZoningType { get; init; }
        public bool IsCornerLand { get; init; }
    }
}
