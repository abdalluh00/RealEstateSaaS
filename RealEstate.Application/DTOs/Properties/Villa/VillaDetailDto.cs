using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Villa
{
    public sealed record VillaDetailDto : PropertyDetailDto
    {
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public int? Floors { get; init; }
        public int? LivingRooms { get; init; }
        public bool HasMaidRoom { get; init; }
        public bool HasDriverRoom { get; init; }
        public bool HasPool { get; init; }
        public bool HasGarden { get; init; }
        public decimal? GardenArea { get; init; }
        public bool HasElevator { get; init; }
        public bool HasMosque { get; init; }
        public bool HasMajlis { get; init; }
        public bool HasStorage { get; init; }
        public bool HasCCTV { get; init; }
        public bool HasGenerator { get; init; }
        public string? FurnishedStatus { get; init; }
    }
}
