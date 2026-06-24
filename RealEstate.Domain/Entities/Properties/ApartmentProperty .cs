using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public class ApartmentProperty : Property
    {
        // ── Unit Info ─────────────────────────────────────
        // null = standalone apartment, filled = inside building ("101", "A2")
        // UnitNumber already on base Property — remove from here

        // ── Rooms ─────────────────────────────────────────
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int? LivingRooms { get; set; }
        public int? FloorNumber { get; set; }
        // nullable — standalone apartment may not have a floor number

        // ── Features ──────────────────────────────────────
        public bool HasMaidRoom { get; set; } = false;
        public bool HasElevator { get; set; } = false;
        public bool HasCentralAC { get; set; } = false;
        public bool HasBalcony { get; set; } = false;
        public bool HasStorage { get; set; } = false;    // مستودع داخل الشقة

        // ── Furnished Status ──────────────────────────────
        public FurnishedStatus? FurnishedStatus { get; set; }
        // Furnished, Unfurnished, SemiFurnished
    }
}