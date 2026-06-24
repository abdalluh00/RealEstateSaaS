using RealEstate.Domain.Common;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Domain.Entities.Properties
{
    public class PropertyDocument : BaseEntity
    {
        // ── Document Info ─────────────────────────────────
        public PropertyDocumentType DocumentType { get; set; }
        // Deed, BuildingPermit, CompletionCertificate, RegaLicense, LandSurvey, Other

        public string DocumentName { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string? DocumentNumber { get; set; }

        // ── Dates ─────────────────────────────────────────
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        // important — REGA license and permits expire

        // ── File Info ─────────────────────────────────────
        public string? FileName { get; set; }
        public long? FileSizeInBytes { get; set; }
        public string? MimeType { get; set; }

        public string? Notes { get; set; }

        // ── Relations ─────────────────────────────────────
        public Guid PropertyId { get; set; }    // FK only
        public Guid CompanyId { get; set; }     // tenant isolation

        // No navigation properties — query by PropertyId directly
    }
}