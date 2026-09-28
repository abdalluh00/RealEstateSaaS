namespace RealEstate.Application.DTOs.Properties.Base
{
    public sealed record PropertyMediaDto
    {
        public Guid Id { get; init; }
        public string MediaUrl { get; init; } = string.Empty;
        public string MediaType { get; init; } = string.Empty;
        public string? Title { get; init; }
        public string? Description { get; init; }
        public bool IsCover { get; init; }
        public int SortOrder { get; init; }
        public string? FileName { get; init; }
        public long? FileSizeInBytes { get; init; }
    }
}