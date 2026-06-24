using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Land
{
    public record LandDetailDto : PropertyDetailDto
    {
        public decimal? StreetWidth { get; init; }
        public int? NumberOfStreets { get; init; }
        public string? ZoningType { get; init; }
        public string? LandShape { get; init; }
        public bool IsCornerLand { get; init; }
        public bool IsWalled { get; init; }
        public bool HasElectricity { get; init; }
        public bool HasWater { get; init; }
        public bool HasSewer { get; init; }
    }
}
