namespace RealEstate.Application.Features.Users.DTO
{
    public record UserDto(
        Guid Id,
        string FullName,
        string Email,
        string Phone,
        string Role,
        bool IsActive,
        DateTime CreatedAt
    );
}
