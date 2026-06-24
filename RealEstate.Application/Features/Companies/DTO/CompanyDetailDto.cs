namespace RealEstate.Application.Features.Companies.DTO
{
    public record CompanyDetailDto(
         Guid Id,
         string Name,
         string Phone,
         string? Logo,
         string? Address,
         string SubscriptionPlan,
         DateTime SubscriptionExpiry,
         bool IsActive,
         bool IsExpired,
         int TotalUsers,
         int TotalProperties,
         int TotalClients,
         int TotalContracts,
         int ActiveContracts
     );
}
