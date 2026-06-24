using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Office
{
    public record OfficeDetailDto : PropertyDetailDto
    {
        public int? FloorNumber { get; init; }
        public int? Bathrooms { get; init; }
        public int? OfficesCount { get; init; }
        public int? MeetingRooms { get; init; }
        public bool HasElevator { get; init; }
        public bool HasCentralAC { get; init; }
        public bool HasReceptionArea { get; init; }
        public bool HasKitchen { get; init; }
        public bool HasStorage { get; init; }
        public bool HasCCTV { get; init; }
        public string? FurnishedStatus { get; init; }
    }
}
