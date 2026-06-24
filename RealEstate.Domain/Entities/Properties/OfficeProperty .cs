using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public class OfficeProperty : Property
    {
        // ── Office Info ───────────────────────────────────
        public int? FloorNumber { get; set; }
        // nullable — standalone office may not have floor number

        public int? Bathrooms { get; set; }
        public int? OfficesCount { get; set; }
        public int? MeetingRooms { get; set; }

        // ── Features ──────────────────────────────────────
        public bool HasElevator { get; set; } = false;
        public bool HasCentralAC { get; set; } = false;
        public bool HasReceptionArea { get; set; } = false;
        public bool HasKitchen { get; set; } = false;      // مطبخ / استراحة
        public bool HasStorage { get; set; } = false;      // مستودع
        public bool HasCCTV { get; set; } = false;         // كاميرات مراقبة

        // ── Furnished Status ──────────────────────────────
        public FurnishedStatus? FurnishedStatus { get; set; }
        // Furnished, Unfurnished, SemiFurnished
    }
}