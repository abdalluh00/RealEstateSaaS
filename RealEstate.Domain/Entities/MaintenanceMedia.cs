using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities
{
    public class MaintenanceMedia : BaseEntity
    {
        // ── File Info ─────────────────────────────────────
        public string FileUrl { get; set; } = string.Empty;
        public string? FileName { get; set; }
        public long? FileSizeInBytes { get; set; }
        public string? MimeType { get; set; }

        public MaintenanceMediaType MediaType { get; set; }
        // Image, Video

        // ── Upload Info ───────────────────────────────────
        public UploadedByType UploadedByType { get; set; }
        // Client, User

        public Guid UploadedById { get; set; }
        // FK to either Client.Id or User.Id depending on UploadedByType

        // ── Stage ─────────────────────────────────────────
        public MediaStage Stage { get; set; } = MediaStage.Before;
        // Before, After — photo before fix vs after fix

        // ── Relations ─────────────────────────────────────
        public Guid MaintenanceRequestId { get; set; }  // FK only, no navigation
        public Guid CompanyId { get; set; }              // tenant isolation
    }
}