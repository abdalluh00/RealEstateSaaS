using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid
{
    public sealed class MarkPaymentPaidCommandHandler
        : IRequestHandler<MarkPaymentPaidCommand, ApiResponse<bool>>
    {
        private readonly IPaymentRepository _payments;
        private readonly IUnitOfWork _uow;

        public MarkPaymentPaidCommandHandler(
            IPaymentRepository payments,
            IUnitOfWork uow)
        {
            _payments = payments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            MarkPaymentPaidCommand cmd,
            CancellationToken ct)
        {
            var payment = await _payments.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (payment is null)
                throw new NotFoundException("الدفعة", cmd.Id);

            if (payment.PaymentStatus == PaymentStatus.Paid)
                throw new ConflictException("الدفعة مدفوعة بالفعل");

            if (payment.PaymentStatus == PaymentStatus.Cancelled)
                throw new ConflictException("لا يمكن تسجيل دفع لدفعة ملغاة");

            payment.PaymentStatus = PaymentStatus.Paid;
            payment.PaidDate = cmd.PaidDate;
            payment.PaymentMethod = cmd.PaymentMethod;
            payment.Reference = cmd.Reference;
            payment.Notes = cmd.Notes;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم تسجيل الدفع بنجاح");
        }
    }
}