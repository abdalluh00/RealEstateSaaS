using FluentValidation;
using RealEstate.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Contracts.Commands.UpdateContract
{
    public class UpdateContractCommandValidator : AbstractValidator<UpdateContractCommand>
    {
        public UpdateContractCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف العقد مطلوب");

            RuleFor(x => x.ContractNumber)
                .NotEmpty().WithMessage("رقم العقد مطلوب")
                .MaximumLength(100).WithMessage("رقم العقد يجب ألا يتجاوز 100 حرف");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("قيمة العقد يجب أن تكون أكبر من صفر");

            RuleFor(x => x.Commission)
                .GreaterThanOrEqualTo(0).WithMessage("العمولة يجب ألا تكون أقل من صفر");

            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("العقار مطلوب");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("العميل مطلوب");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("الوسيط مطلوب");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("تاريخ بداية العقد مطلوب");

            When(x => x.ContractType == ContractType.Rent, () =>
            {
                RuleFor(x => x.EndDate)
                    .NotNull().WithMessage("تاريخ نهاية عقد الإيجار مطلوب");

                RuleFor(x => x.PaymentCycle)
                    .NotNull().WithMessage("دورة السداد مطلوبة لعقد الإيجار");
            });

            When(x => x.ContractType == ContractType.Sale, () =>
            {
                RuleFor(x => x.PaymentMethod)
                    .NotNull().WithMessage("طريقة الدفع مطلوبة لعقد البيع");
            });

            When(x => x.ContractType == ContractType.Sale, () =>
            {
                RuleFor(x => x.SecurityDeposit)
                    .Must(x => x == null)
                    .WithMessage("مبلغ التأمين غير مسموح في عقد البيع");
            });

            When(x => x.CommissionType == CommissionType.Percentage, () =>
            {
                RuleFor(x => x.Commission)
                    .InclusiveBetween(0, 100)
                    .WithMessage("نسبة العمولة يجب أن تكون بين 0 و 100");
            });

            RuleFor(x => x)
                .Must(x => x.EndDate == null || x.EndDate >= x.StartDate)
                .WithMessage("تاريخ نهاية العقد يجب أن يكون بعد أو يساوي تاريخ البداية");
        }
    }
}
