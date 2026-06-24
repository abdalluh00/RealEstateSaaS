using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;
namespace RealEstate.Application.Features.Payments.Commands.CancelPayment
{
    public class CancelPaymentHandler : IRequestHandler<CancelPaymentCommand, ApiResponse<bool>>
    {
        private readonly IPaymentRepository _repo;

        public CancelPaymentHandler(IPaymentRepository repo) => _repo = repo;

        public async Task<ApiResponse<bool>> Handle(
            CancelPaymentCommand request,
            CancellationToken ct)
        {
            var payment = await _repo.GetByIdAsync(request.PaymentId);

            if (payment is null)
                throw new NotFoundException("الدفعة", request.PaymentId);

            if (payment.Status == "Paid")
                throw new ConflictException("لا يمكن إلغاء دفعة مدفوعة");

            payment.Status = "Cancelled";

            _repo.Update(payment);
            await _repo.SaveChangesAsync();

            return ApiResponse<bool>.Ok(true, "تم إلغاء الدفعة بنجاح");
        }
    }
}
