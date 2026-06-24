namespace RealEstate.Domain.Entities.Properties
{
    public class BuildingProperty : Property
    {
        // ── Building Info ─────────────────────────────────
        public int? TotalFloors { get; set; }
        public int? UnitsCount { get; set; }        // total units capacity
        public int? BasementFloors { get; set; }    // عدد الأدوار السفلية

        // ── Features ──────────────────────────────────────
        public bool HasElevator { get; set; } = false;
        public bool HasParkingFloor { get; set; } = false;
        public bool HasMosque { get; set; } = false;       // مسجد — common in KSA
        public bool HasGuard { get; set; } = false;        // حارس أمن
        public bool HasGenerator { get; set; } = false;    // مولد كهربائي
        public bool HasCCTV { get; set; } = false;         // كاميرات مراقبة

        // BuildingProperty never has ParentPropertyId filled
        // Units (ApartmentProperty, OfficeProperty etc.) queried by ParentPropertyId
    }
}