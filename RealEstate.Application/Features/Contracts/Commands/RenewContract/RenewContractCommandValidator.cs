using FluentValidation;

namespace RealEstate.Application.Features.Contracts.Commands.RenewContract
{
    public sealed class RenewContractCommandValidator
        : AbstractValidator<RenewContractCommand>
    {
        public RenewContractCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف العقد مطلوب");

            RuleFor(x => x.NewStartDate)
                .NotEmpty().WithMessage("تاريخ البداية الجديد مطلوب");

            RuleFor(x => x.NewEndDate)
                .GreaterThan(x => x.NewStartDate)
                .WithMessage("تاريخ الانتهاء يجب أن يكون بعد تاريخ البداية");

            RuleFor(x => x.NewAmount)
                .GreaterThan(0).WithMessage("المبلغ يجب أن يكون أكبر من صفر");
        }
    }
}