using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;

namespace RealEstate.Application.Common.Interfaces
{
    public interface IPaymentGeneratorService
    {
        IReadOnlyList<Payment> Generate(
            Guid contractId,
            Guid companyId,
            decimal totalAmount,
            DateTime startDate,
            DateTime endDate,
            PaymentCycle paymentCycle);
    }
}