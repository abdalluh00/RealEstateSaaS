using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public class WarehouseProperty : Property
    {
        // ── Warehouse Info ────────────────────────────────
        public decimal? CeilingHeight { get; set; }        // ارتفاع السقف بالمتر
        public int? LoadingDocks { get; set; }              // بوابات التحميل
        public int? GateCount { get; set; }                 // عدد البوابات

        // ── Electricity ───────────────────────────────────
        public ElectricityCapacity ElectricityCapacity { get; set; }
        // V220, V380, V440 — enum instead of free text

        // ── Features ──────────────────────────────────────
        public bool HasOfficeSpace { get; set; } = false;  // مكتب إداري داخل المستودع
        public bool HasSecurityRoom { get; set; } = false;  // غرفة أمن
        public bool HasCCTV { get; set; } = false;          // كاميرات مراقبة
        public bool HasFireSystem { get; set; } = false;    // نظام إطفاء حريق
        public bool HasColdStorage { get; set; } = false;   // تبريد — ثلاجات
        public bool HasMosanada { get; set; } = false;      // رافعة شوكية / مسانده
        public bool IsFenced { get; set; } = false;         // محاط بسور
        public bool HasTruckAccess { get; set; } = false;   // مدخل للشاحنات الكبيرة
    }
}