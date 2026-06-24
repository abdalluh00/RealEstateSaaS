
namespace RealEstate.Application.Features.Companies.DTO
{
    public record CompanyDto(
        Guid Id,
        string Name,
        string Phone,
        string? Logo,
        string SubscriptionPlan,
        DateTime SubscriptionExpiry,
        bool IsActive,
        bool IsExpired,
        int TotalUsers,
        int TotalProperties
    );
}
