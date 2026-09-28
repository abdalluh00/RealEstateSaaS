namespace RealEstate.Application.DTOs.Companies
{
    public sealed record CompanyDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Logo { get; init; }
        public string? Address { get; init; }
        public string Phone { get; init; } = string.Empty;
        public string SubscriptionPlan { get; init; } = string.Empty;
        public DateTime SubscriptionExpiry { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}