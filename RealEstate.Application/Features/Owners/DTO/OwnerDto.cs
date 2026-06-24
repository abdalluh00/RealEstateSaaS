
namespace RealEstate.Application.Features.Owners.DTO
{
    public record OwnerDto(
        Guid Id,
        string FullName,
        string Phone,
        string? Email,
        string? IdNumber,
        int TotalProperties,
        int ActiveContracts,
        DateTime CreatedAt
    );
}
