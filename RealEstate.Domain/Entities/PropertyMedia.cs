using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities.Properties;

namespace RealEstate.Domain.Entities
{
    public class PropertyMedia : BaseEntity
    {
        // ── Media Info ────────────────────────────────────
        public string MediaUrl { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }

        // ── Display ───────────────────────────────────────
        public bool IsCover { get; set; } = false;
        public int SortOrder { get; set; } = 0;

        // ── File Info ─────────────────────────────────────
        public string? FileName { get; set; }
        public long? FileSizeInBytes { get; set; }
        public string? MimeType { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid PropertyId { get; set; }       // FK only, no navigation
        public Guid CompanyId { get; set; }        // tenant isolation
    }
}