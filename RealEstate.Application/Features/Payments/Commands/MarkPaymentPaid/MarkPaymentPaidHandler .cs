using MediatR;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid
{
    public class MarkPaymentPaidHandler : IRequestHandler<MarkPaymentPaidCommand, ApiResponse<bool>>
    {
        private readonly IPaymentRepository _repo;
        private readonly IWhatsAppService _whatsApp;

        public MarkPaymentPaidHandler(IPaymentRepository repo, IWhatsAppService whatsApp)
        {
            _repo = repo;
            _whatsApp = whatsApp;
        }

        public async Task<ApiResponse<bool>> Handle(
            MarkPaymentPaidCommand request,
            CancellationToken ct)
        {
            var payment = await _repo.GetByIdAsync(request.PaymentId);

            if (payment is null)
                throw new NotFoundException("الدفعة", request.PaymentId);

            if (payment.Status == "Paid")
                throw new ConflictException("هذه الدفعة مدفوعة مسبقاً");

            payment.Status = "Paid";
            payment.PaidDate = DateTime.UtcNow;
            payment.Method = request.Method;
            payment.Reference = request.Reference;

            _repo.Update(payment);
            await _repo.SaveChangesAsync();

            // إرسال إيصال للعميل
            _ = _whatsApp.SendAsync(
                payment.Contract.Client.Phone,
                $"""
                 ✅ تم استلام دفعتك بنجاح

                💰 المبلغ: {payment.Amount:N0} ريال
                📅 تاريخ الدفع: {DateTime.UtcNow:dd/MM/yyyy}
                💳 طريقة الدفع: {request.Method}

                 شكراً لك! 🏠
                """
            );

            return ApiResponse<bool>.Ok(true, "تم تسجيل الدفعة بنجاح");
        }
    }
}
