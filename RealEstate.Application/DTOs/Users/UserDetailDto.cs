namespace RealEstate.Application.DTOs.Users
{
    public sealed record UserDetailDto
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public bool IsInvitationAccepted { get; init; }
        public DateTime? InvitationExpiry { get; init; }
        public DateTime? InvitationAcceptedAt { get; init; }
        public DateTime? LastLoginAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}