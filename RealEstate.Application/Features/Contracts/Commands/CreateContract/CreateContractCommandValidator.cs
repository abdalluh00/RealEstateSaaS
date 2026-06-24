using FluentValidation;
using RealEstate.Domain.Common.Enums;
namespace RealEstate.Application.Features.Contracts.Commands.CreateContract
{
    public class CreateContractCommandValidator : AbstractValidator<CreateContractCommand>
    {
        public class CreateContractValidator : AbstractValidator<CreateContractCommand>
        {
            public CreateContractValidator()
            {
                RuleFor(x => x.PropertyId)
                    .NotEmpty().WithMessage("العقار مطلوب");

                RuleFor(x => x.ClientId)
                    .NotEmpty().WithMessage("العميل مطلوب");

                RuleFor(x => x.AgentId)
                    .NotEmpty().WithMessage("الوسيط مطلوب");

                RuleFor(x => x.ContractType)
                    .IsInEnum().WithMessage("نوع العقد غير صالح");

                RuleFor(x => x.Amount)
                    .GreaterThan(0).WithMessage("قيمة العقد يجب أن تكون أكبر من صفر");

                RuleFor(x => x.CommissionType)
                    .IsInEnum().WithMessage("نوع العمولة غير صالح");

                RuleFor(x => x.Commission)
                    .GreaterThanOrEqualTo(0).WithMessage("العمولة يجب أن تكون صفر أو أكبر");

                RuleFor(x => x.CommissionStatus)
                    .IsInEnum().WithMessage("حالة العمولة غير صالحة");

                RuleFor(x => x.StartDate)
                    .NotEmpty().WithMessage("تاريخ بداية العقد مطلوب");

                RuleFor(x => x.Notes)
                    .MaximumLength(2000)
                    .WithMessage("الملاحظات يجب ألا تتجاوز 2000 حرف");

                RuleFor(x => x)
                    .Must(x => x.ContractType != ContractType.Rent || x.EndDate.HasValue)
                    .WithMessage("تاريخ نهاية العقد مطلوب في عقود الإيجار");

                RuleFor(x => x)
                    .Must(x => x.ContractType != ContractType.Rent || x.PaymentCycle.HasValue)
                    .WithMessage("دورية الدفع مطلوبة في عقود الإيجار");

                RuleFor(x => x)
                    .Must(x => x.ContractType != ContractType.Rent ||
                               !x.EndDate.HasValue ||
                               x.EndDate.Value.Date > x.StartDate.Date)
                    .WithMessage("تاريخ نهاية العقد يجب أن يكون بعد تاريخ البداية");

                RuleFor(x => x)
                    .Must(x => x.ContractType != ContractType.Sale || x.PaymentMethod.HasValue)
                    .WithMessage("طريقة الدفع مطلوبة في عقود البيع");

                RuleFor(x => x.SecurityDeposit)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.SecurityDeposit.HasValue)
                    .WithMessage("مبلغ التأمين يجب أن يكون صفر أو أكبر");
            }
        }
    }
}
