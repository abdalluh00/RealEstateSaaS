using FluentValidation;

namespace RealEstate.Application.Features.Payments.Commands.MarkPaymentPaid
{
    public class MarkPaymentPaidValidator : AbstractValidator<MarkPaymentPaidCommand>
    {
        private static readonly string[] AllowedMethods =
            ["Cash", "Bank", "Online", "Mada", "STC"];

        public MarkPaymentPaidValidator()
        {
            RuleFor(x => x.PaymentId).NotEmpty();

            RuleFor(x => x.Method)
                .NotEmpty().WithMessage("طريقة الدفع مطلوبة")
                .Must(m => AllowedMethods.Contains(m))
                .WithMessage("طريقة الدفع غير صحيحة — Cash, Bank, Online, Mada, STC");
        }
    }
}
