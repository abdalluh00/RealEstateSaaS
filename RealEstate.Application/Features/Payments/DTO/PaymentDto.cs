namespace RealEstate.Application.Features.Payments.DTO
{
    public record PaymentDto(
       Guid Id,
       decimal Amount,
       DateTime DueDate,
       DateTime? PaidDate,
       string Status,
       string? Method,
       string? Reference,
       int DaysOverdue
   );
}
