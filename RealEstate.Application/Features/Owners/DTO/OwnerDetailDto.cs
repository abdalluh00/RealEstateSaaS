
namespace RealEstate.Application.Features.Owners.DTO
{
    public record OwnerDetailDto(
        Guid Id,
        string FullName,
        string Phone,
        string? Email,
        string? IdNumber,
        string? Notes,
        List<OwnerPropertyDto> Properties
    );
}
