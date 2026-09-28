using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Villas.Commands.UpdateVilla
{
    public class UpdateVillaCommandValidator : AbstractValidator<UpdateVillaCommand>
    {
        public UpdateVillaCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الفيلا مطلوب");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان العقار مطلوب")
                .MaximumLength(300).WithMessage("عنوان العقار يجب ألا يتجاوز 300 حرف");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("المدينة مطلوبة")
                .MaximumLength(100).WithMessage("اسم المدينة يجب ألا يتجاوز 100 حرف");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("الحي مطلوب")
                .MaximumLength(100).WithMessage("اسم الحي يجب ألا يتجاوز 100 حرف");

            RuleFor(x => x.Address)
                .MaximumLength(500).When(x => !string.IsNullOrWhiteSpace(x.Address))
                .WithMessage("العنوان يجب ألا يتجاوز 500 حرف");

            RuleFor(x => x.RegaLicenseNumber)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.RegaLicenseNumber));

            RuleFor(x => x.DeedNumber)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.DeedNumber));

            RuleFor(x => x.MunicipalityNumber)
                .MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.MunicipalityNumber));

            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("المالك مطلوب");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("الوكيل مطلوب");

            RuleFor(x => x.Bedrooms)
                .GreaterThan(0).WithMessage("عدد غرف النوم يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Bathrooms)
                .GreaterThan(0).WithMessage("عدد دورات المياه يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Floors)
                .GreaterThan(0).WithMessage("عدد الأدوار يجب أن يكون أكبر من صفر");

            RuleFor(x => x.GardenArea)
                .GreaterThan(0)
                .When(x => x.GardenArea.HasValue)
                .WithMessage("مساحة الحديقة يجب أن تكون أكبر من صفر");

            RuleFor(x => x.UnitNumber)
               .NotEmpty().WithMessage("رقم الوحدة مطلوب عند وجود عقار أب")
               .When(x => x.ParentPropertyId.HasValue);
        }
    }
}
