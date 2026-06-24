using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Building
{
    public record BuildingDetailDto : PropertyDetailDto
    {
        public int? TotalFloors { get; init; }
        public int? UnitsCount { get; init; }
        public bool HasElevator { get; init; }
        public bool HasParkingFloor { get; init; }
        public bool HasMosque { get; init; }
        public bool HasGuard { get; init; }
        public bool HasGenerator { get; init; }
        public bool HasCCTV { get; init; }
        public int? BasementFloors { get; init; }
        public IEnumerable<PropertyListDto> Units { get; init; } = [];
        // all units (apartments, offices, shops) inside this building
    }
}
