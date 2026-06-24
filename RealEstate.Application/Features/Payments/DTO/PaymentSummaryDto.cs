
namespace RealEstate.Application.Features.Payments.DTO
{
    public record PaymentSummaryDto(
       decimal TotalExpected,
       decimal TotalCollected,
       decimal TotalOverdue,
       decimal TotalPending,
       int OverdueCount,
       int PendingCount
   );
}
