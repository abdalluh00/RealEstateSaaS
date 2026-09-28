using FluentValidation;

namespace RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid
{
    public sealed class MarkPaymentPaidCommandValidator
        : AbstractValidator<MarkPaymentPaidCommand>
    {
        public MarkPaymentPaidCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الدفعة مطلوب");

            RuleFor(x => x.PaidDate)
                .NotEmpty().WithMessage("تاريخ الدفع مطلوب")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("تاريخ الدفع لا يمكن أن يكون في المستقبل");

            RuleFor(x => x.PaymentMethod)
                .IsInEnum().WithMessage("طريقة الدفع غير صحيحة");
        }
    }
}