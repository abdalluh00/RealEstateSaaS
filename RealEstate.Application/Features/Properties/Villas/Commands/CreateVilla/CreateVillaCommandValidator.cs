using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.CreateVilla
{
    public class CreateVillaCommandValidator : AbstractValidator<CreateVillaCommand>
    {
        public CreateVillaCommandValidator()
        {
            RuleFor(x => x.Bedrooms)
     .GreaterThan(0).WithMessage("عدد غرف النوم يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Bathrooms)
                .GreaterThan(0).WithMessage("عدد دورات المياه يجب أن يكون أكبر من صفر");

            RuleFor(x => x.GardenArea)
                .GreaterThan(0).When(x => x.GardenArea.HasValue)
                .WithMessage("مساحة الحديقة يجب أن تكون أكبر من صفر");

            RuleFor(x => x.UnitNumber)
                .NotEmpty().WithMessage("رقم الوحدة مطلوب عند وجود عقار أب")
                .When(x => x.ParentPropertyId.HasValue);
        }
    }
}
