namespace RealEstate.Application.DTOs.Users
{
    public sealed record LoginResponseDto
    {
        public string Token { get; init; } = string.Empty;
        public Guid UserId { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Role { get; init; } = string.Empty;
        public Guid CompanyId { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}