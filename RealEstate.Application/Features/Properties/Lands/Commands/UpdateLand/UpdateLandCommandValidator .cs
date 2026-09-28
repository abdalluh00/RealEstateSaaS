using FluentValidation;

namespace RealEstate.Application.Features.Lands.Commands.UpdateLand
{
    public sealed class UpdateLandCommandValidator
        : AbstractValidator<UpdateLandCommand>
    {
        public UpdateLandCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الأرض مطلوب");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان العقار مطلوب")
                .MaximumLength(200).WithMessage("العنوان لا يتجاوز 200 حرف");

            RuleFor(x => x.Purpose)
                .IsInEnum().WithMessage("نوع الغرض غير صحيح");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("المدينة مطلوبة");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("الحي مطلوب");
        }
    }
}