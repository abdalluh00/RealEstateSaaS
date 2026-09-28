namespace RealEstate.Application.DTOs.Maintenance
{
    public sealed record MaintenanceListDto
    {
        public Guid Id { get; init; }
        public string RequestNumber { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Priority { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public Guid? PropertyId { get; init; }
        public string? PropertyTitle { get; init; }
        public string? ClientName { get; init; }
        public string? AssignedToName { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}