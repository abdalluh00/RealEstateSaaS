using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties.Warehouse
{
    public record WarehouseListDto : PropertyListDto
    {
        public decimal? CeilingHeight { get; init; }
        public int? LoadingDocks { get; init; }
        public string? ElectricityCapacity { get; init; }
        public bool HasColdStorage { get; init; }
    }
}
