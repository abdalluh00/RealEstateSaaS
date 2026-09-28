using FluentValidation;

namespace RealEstate.Application.Features.Cheques.Commands.UpdateCheque
{
    public sealed class UpdateChequeCommandValidator
        : AbstractValidator<UpdateChequeCommand>
    {
        public UpdateChequeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("الشيك مطلوب");

            RuleFor(x => x.ChequeNumber)
                .NotEmpty()
                .WithMessage("رقم الشيك مطلوب")
                .MaximumLength(100)
                .WithMessage("رقم الشيك يجب ألا يتجاوز 100 حرف");

            RuleFor(x => x.BankName)
                .NotEmpty()
                .WithMessage("اسم البنك مطلوب")
                .MaximumLength(200)
                .WithMessage("اسم البنك يجب ألا يتجاوز 200 حرف");

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("مبلغ الشيك يجب أن يكون أكبر من صفر");

            RuleFor(x => x.DueDate)
                .NotEmpty()
                .WithMessage("تاريخ الاستحقاق مطلوب");

            RuleFor(x => x.ChequeOrder)
                .GreaterThan(0)
                .WithMessage("ترتيب الشيك يجب أن يكون أكبر من صفر");
        }
    }
}