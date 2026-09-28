using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Apartment
{
    public sealed record ApartmentDetailDto : PropertyDetailDto
    {
        public int Bedrooms { get; init; }
        public int Bathrooms { get; init; }
        public int? FloorNumber { get; init; }
        public int? LivingRooms { get; init; }
        public bool HasMaidRoom { get; init; }
        public bool HasElevator { get; init; }
        public bool HasCentralAC { get; init; }
        public bool HasBalcony { get; init; }
        public bool HasStorage { get; init; }
        public string? FurnishedStatus { get; init; }
    }
}
