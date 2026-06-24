using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Apartment
{
    public sealed record ApartmentListDto : PropertyListDto
    {
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public int? FloorNumber { get; init; }
        public bool HasElevator { get; init; }
        public string? FurnishedStatus { get; init; }
    }
}
