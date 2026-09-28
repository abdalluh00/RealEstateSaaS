using FluentValidation;

namespace RealEstate.Application.Features.Cheques.Commands.BounceCheque
{
    public sealed class BounceChequeCommandValidator
        : AbstractValidator<BounceChequeCommand>
    {
        public BounceChequeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("الشيك مطلوب");

            RuleFor(x => x.BounceReason)
                .NotEmpty()
                .WithMessage("سبب ارتجاع الشيك مطلوب")
                .MaximumLength(500)
                .WithMessage("سبب الارتجاع يجب ألا يتجاوز 500 حرف");
        }
    }
}