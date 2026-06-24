using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public class LandProperty : Property
    {
        // ── Land Info ─────────────────────────────────────
        public decimal? StreetWidth { get; set; }      // عرض الشارع بالمتر
        public int? NumberOfStreets { get; set; }       // عدد الشوارع

        // ── Zoning ────────────────────────────────────────
        public ZoningType? ZoningType { get; set; }
        // Residential, Commercial, Industrial, Agricultural

        // ── Features ──────────────────────────────────────
        public bool IsCornerLand { get; set; } = false;  // أرض زاوية
        public bool IsWalled { get; set; } = false;       // محاطة بسور
        public bool HasElectricity { get; set; } = false; // متصلة بالكهرباء
        public bool HasWater { get; set; } = false;        // متصلة بالماء
        public bool HasSewer { get; set; } = false;        // متصلة بالصرف الصحي

        // ── Shape ─────────────────────────────────────────
        public LandShape? LandShape { get; set; }
        // Square, Rectangular, Irregular
    }
}