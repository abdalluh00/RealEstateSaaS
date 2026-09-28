using FluentValidation;
namespace RealEstate.Application.Features.Properties.Apartments.Commands.CreateApartment
{
    public sealed class CreateApartmentCommandValidator
        : AbstractValidator<CreateApartmentCommand>
    {
        public CreateApartmentCommandValidator()
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

            RuleFor(x => x.Bedrooms)
                .GreaterThan(0).WithMessage("عدد غرف النوم يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Bathrooms)
                .GreaterThan(0).WithMessage("عدد دورات المياه يجب أن يكون أكبر من صفر");

            // UnitNumber required only when inside a parent
            RuleFor(x => x.UnitNumber)
                .NotEmpty().WithMessage("رقم الوحدة مطلوب عند وجود عقار أب")
                .When(x => x.ParentPropertyId.HasValue);
        }
    }
}
