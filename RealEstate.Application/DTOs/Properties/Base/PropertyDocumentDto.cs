namespace RealEstate.Application.DTOs.Properties.Base
{
    public sealed record PropertyDocumentDto
    {
        public Guid Id { get; init; }
        public string DocumentType { get; init; } = string.Empty;
        public string DocumentName { get; init; } = string.Empty;
        public string FileUrl { get; init; } = string.Empty;
        public string? DocumentNumber { get; init; }
        public DateTime? IssueDate { get; init; }
        public DateTime? ExpiryDate { get; init; }
        public string? FileName { get; init; }
        public long? FileSizeInBytes { get; init; }
        public string? Notes { get; init; }
    }
}