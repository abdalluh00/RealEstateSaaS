using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public class VillaProperty : Property
    {
        // ── Rooms ─────────────────────────────────────────
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int? LivingRooms { get; set; }
        public int? Floors { get; set; }
        // nullable — compound villa unit may not need this

        // ── Features ──────────────────────────────────────
        public bool HasMaidRoom { get; set; } = false;
        public bool HasDriverRoom { get; set; } = false;
        public bool HasPool { get; set; } = false;
        public bool HasGarden { get; set; } = false;
        public decimal? GardenArea { get; set; }        // m² — filled only if HasGarden
        public bool HasElevator { get; set; } = false;  // multi-floor villas
        public bool HasMosque { get; set; } = false;    // مصلى داخلي — common in KSA
        public bool HasMajlis { get; set; } = false;    // مجلس — very common in KSA
        public bool HasStorage { get; set; } = false;   // مستودع
        public bool HasCCTV { get; set; } = false;
        public bool HasGenerator { get; set; } = false;

        // ── Furnished Status ──────────────────────────────
        public FurnishedStatus FurnishedStatus { get; set; }
        // Furnished, Unfurnished, SemiFurnished
    }
}