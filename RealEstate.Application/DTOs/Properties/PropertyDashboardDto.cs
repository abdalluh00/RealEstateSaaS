using RealEstate.Application.DTOs.Properties.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.DTOs.Properties
{
    public sealed record PropertyDashboardDto
    {
        // ── Stats ─────────────────────────────────────────
        public int TotalProperties { get; init; }
        public int Available { get; init; }
        public int Rented { get; init; }
        public int Sold { get; init; }
        public int Reserved { get; init; }
        public int Featured { get; init; }
        public int Published { get; init; }

        // ── Top Featured (widget) ─────────────────────────
        public IReadOnlyList<PropertyListDto> TopFeatured { get; init; } = [];
    }
}
