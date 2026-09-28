using RealEstate.Application.Common.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Services
{
    public class PaymentGeneratorService : IPaymentGeneratorService
    {
        public IReadOnlyList<Payment> Generate(
            Guid contractId,
            Guid companyId,
            decimal totalAmount,
            DateTime startDate,
            DateTime endDate,
            PaymentCycle paymentCycle)
        {
            var payments = new List<Payment>();
            var intervalMonths = paymentCycle switch
            {
                PaymentCycle.Monthly => 1,
                PaymentCycle.Quarterly => 3,
                PaymentCycle.SemiAnnual => 6,
                PaymentCycle.Annual => 12,
                _ => 1
            };

            var current = startDate;
            var number = 1;

            while (current <= endDate)
            {
                payments.Add(new Payment
                {
                    ContractId = contractId,
                    CompanyId = companyId,
                    PaymentNumber = number++,
                    Amount = totalAmount,
                    DueDate = current,
                    PaymentStatus = PaymentStatus.Pending
                });

                current = current.AddMonths(intervalMonths);
            }

            return payments;
        }
    }
}