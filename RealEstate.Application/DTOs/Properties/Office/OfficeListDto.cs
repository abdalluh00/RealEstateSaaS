using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Office
{
    public record OfficeListDto : PropertyListDto
    {
        public int? FloorNumber { get; init; }
        public int? OfficesCount { get; init; }
        public bool HasElevator { get; init; }
        public string? FurnishedStatus { get; init; }
    }
}
