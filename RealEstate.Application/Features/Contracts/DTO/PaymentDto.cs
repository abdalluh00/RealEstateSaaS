
namespace RealEstate.Application.Features.Contracts.DTO
{
    public record PaymentDto(
         Guid Id,
         decimal Amount,
         DateTime DueDate,
         DateTime? PaidDate,
         string Status,
         string? Method
     );
}
