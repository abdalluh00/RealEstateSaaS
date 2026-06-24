using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Warehouse
{
    public record WarehouseDetailDto : PropertyDetailDto
    {
        public decimal? CeilingHeight { get; init; }
        public int? LoadingDocks { get; init; }
        public int? GateCount { get; init; }
        public string? ElectricityCapacity { get; init; }
        public bool HasOfficeSpace { get; init; }
        public bool HasSecurityRoom { get; init; }
        public bool HasCCTV { get; init; }
        public bool HasFireSystem { get; init; }
        public bool HasColdStorage { get; init; }
        public bool HasMosanada { get; init; }
        public bool IsFenced { get; init; }
        public bool HasTruckAccess { get; init; }
    }
}
