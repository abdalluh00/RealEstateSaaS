using FluentValidation;

namespace RealEstate.Application.Features.Contracts.Commands.CancelContract
{
    public sealed class CancelContractCommandValidator
        : AbstractValidator<CancelContractCommand>
    {
        public CancelContractCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف العقد مطلوب");

            RuleFor(x => x.CancellationReason)
                .NotEmpty().WithMessage("سبب الإلغاء مطلوب")
                .MaximumLength(500).WithMessage("سبب الإلغاء لا يتجاوز 500 حرف");
        }
    }
}