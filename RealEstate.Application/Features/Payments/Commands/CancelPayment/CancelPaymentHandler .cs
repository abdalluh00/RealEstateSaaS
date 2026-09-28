using MediatR;
using RealEstate.Application.Interfaces;
using RealEstate.Domain.Common.Enums;
using RealEstate.Domain.Interfaces;
using RealEstate.Shared.Common;
using RealEstate.Shared.Common.Exceptions;

namespace RealEstate.Application.Features.Payments.Commands.CancelPayment
{
    public sealed class CancelPaymentCommandHandler
        : IRequestHandler<CancelPaymentCommand, ApiResponse<bool>>
    {
        private readonly IPaymentRepository _payments;
        private readonly IUnitOfWork _uow;

        public CancelPaymentCommandHandler(
            IPaymentRepository payments,
            IUnitOfWork uow)
        {
            _payments = payments;
            _uow = uow;
        }

        public async Task<ApiResponse<bool>> Handle(
            CancelPaymentCommand cmd,
            CancellationToken ct)
        {
            var payment = await _payments.GetByIdForCommandAsync(
                cmd.Id, cmd.CompanyId, ct);

            if (payment is null)
                throw new NotFoundException("الدفعة", cmd.Id);

            if (payment.PaymentStatus is PaymentStatus.Paid
                                      or PaymentStatus.Cancelled)
                throw new ConflictException(
                    "لا يمكن إلغاء هذه الدفعة");

            payment.PaymentStatus = PaymentStatus.Cancelled;
            payment.Notes = cmd.Notes;

            await _uow.SaveChangesAsync(ct);

            return ApiResponse<bool>.Ok(true, "تم إلغاء الدفعة بنجاح");
        }
    }
}