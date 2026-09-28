using FluentValidation;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Contracts.Commands.CreateContract
{
    public sealed class CreateContractCommandValidator
        : AbstractValidator<CreateContractCommand>
    {
        public CreateContractCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("العقار مطلوب");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("العميل مطلوب");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("الوكيل مطلوب");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("المبلغ يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Commission)
                .GreaterThanOrEqualTo(0).WithMessage("العمولة لا يمكن أن تكون سالبة");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("تاريخ البداية مطلوب");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.EndDate.HasValue)
                .WithMessage("تاريخ الانتهاء يجب أن يكون بعد تاريخ البداية");

            // Rent contracts must have PaymentCycle
            RuleFor(x => x.PaymentCycle)
                .NotNull().WithMessage("دورة الدفع مطلوبة لعقود الإيجار")
                .When(x => x.ContractType == ContractType.Rent);

            // Rent contracts must have EndDate
            RuleFor(x => x.EndDate)
                .NotNull().WithMessage("تاريخ الانتهاء مطلوب لعقود الإيجار")
                .When(x => x.ContractType == ContractType.Rent);
        }
    }
}