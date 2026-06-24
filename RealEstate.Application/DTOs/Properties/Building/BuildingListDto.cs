using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Building
{
    public record BuildingListDto : PropertyListDto
    {
        public int? TotalFloors { get; init; }
        public int? UnitsCount { get; init; }
        public bool HasElevator { get; init; }
    }
}
