namespace RealEstate.Application.DTOs.Clients
{
    public sealed record ClientListDto
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string? Email { get; init; }
        public string LeadStatus { get; init; } = string.Empty;
        public string? Source { get; init; }
        public string? AssignedAgentName { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}