namespace RealEstate.Application.DTOs.Maintenance
{
    public sealed record MaintenanceMediaDto
    {
        public Guid Id { get; init; }
        public string FileUrl { get; init; } = string.Empty;
        public string MediaType { get; init; } = string.Empty;
        public string Stage { get; init; } = string.Empty;
        public string? FileName { get; init; }
        public long? FileSizeInBytes { get; init; }
    }
}