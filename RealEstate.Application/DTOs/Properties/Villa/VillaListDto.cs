using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Villa
{
    public sealed record VillaListDto : PropertyListDto
    {
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public bool HasPool { get; init; }
        public bool HasMajlis { get; init; }
        public string? FurnishedStatus { get; init; }
    }
}
