namespace RealEstate.Application.Features.Clients.DTO
{
    public record ClientDetailDto(
        Guid Id,
        string FullName,
        string Phone,
        string? Email,
        string LeadStatus,
        string? Source,
        string? Notes,
        
        DateTime CreatedAt
    );
}
