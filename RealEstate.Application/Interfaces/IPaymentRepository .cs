using RealEstate.Domain.Entities;
using RealEstate.Domain.ReadModels;

namespace RealEstate.Domain.Interfaces
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<IEnumerable<PaymentListItem>> GetByContractAsync(Guid contractId);
        Task<IEnumerable<PaymentListItem>> GetOverdueAsync(Guid companyId);
        Task<IEnumerable<PaymentListItem>> GetUpcomingAsync(Guid companyId, int daysAhead);
        Task<PaymentSummary> GetSummaryAsync(Guid companyId);
    }
}
