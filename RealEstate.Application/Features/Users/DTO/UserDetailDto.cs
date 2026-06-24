namespace RealEstate.Application.Features.Users.DTO
{
    public record UserDetailDto(
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        string Role,
        bool IsActive,
        Guid CompanyId,
        string CompanyName,
        DateTime CreatedAt
    );
}
