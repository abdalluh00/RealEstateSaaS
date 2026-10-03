using FluentValidation;

namespace RealEstate.Application.Features.Buildings.Commands.CreateBuilding
{
    public sealed class CreateBuildingCommandValidator
        : AbstractValidator<CreateBuildingCommand>
    {
        public CreateBuildingCommandValidator()
        {
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

            RuleFor(x => x.TotalFloors)
                .GreaterThan(0).When(x => x.TotalFloors.HasValue)
                .WithMessage("عدد الطوابق يجب أن يكون أكبر من صفر");

            RuleFor(x => x.UnitsCount)
                .GreaterThan(0).When(x => x.UnitsCount.HasValue)
                .WithMessage("عدد الوحدات يجب أن يكون أكبر من صفر");

            RuleFor(x => x.BasementFloors)
                .GreaterThanOrEqualTo(0).When(x => x.BasementFloors.HasValue)
                .WithMessage("عدد الأدوار السفلية لا يمكن أن يكون سالباً");
        }
    }
}