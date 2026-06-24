using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Buildings.Commands.CreateBuilding
{
    public class CreateBuildingCommandValidator : AbstractValidator<CreateBuildingCommand>
    {
        public CreateBuildingCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان المبنى مطلوب")
                .MaximumLength(300).WithMessage("عنوان المبنى يجب ألا يتجاوز 300 حرف");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("المدينة مطلوبة")
                .MaximumLength(100).WithMessage("المدينة يجب ألا تتجاوز 100 حرف");

            RuleFor(x => x.District)
                .NotEmpty().WithMessage("الحي مطلوب")
                .MaximumLength(100).WithMessage("الحي يجب ألا يتجاوز 100 حرف");

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("العنوان يجب ألا يتجاوز 500 حرف")
                .When(x => !string.IsNullOrWhiteSpace(x.Address));

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر");

            RuleFor(x => x.Area)
                .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر");

            RuleFor(x => x.OwnerId)
                .NotEmpty().WithMessage("المالك مطلوب");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("الوسيط/الموظف المسؤول مطلوب");

            RuleFor(x => x.FacingDirection)
                .MaximumLength(50).WithMessage("اتجاه الواجهة يجب ألا يتجاوز 50 حرف")
                .When(x => !string.IsNullOrWhiteSpace(x.FacingDirection));

            RuleFor(x => x.RegaLicenseNumber)
                .MaximumLength(100).WithMessage("رقم ترخيص فال يجب ألا يتجاوز 100 حرف")
                .When(x => !string.IsNullOrWhiteSpace(x.RegaLicenseNumber));

            RuleFor(x => x.DeedNumber)
                .MaximumLength(100).WithMessage("رقم الصك يجب ألا يتجاوز 100 حرف")
                .When(x => !string.IsNullOrWhiteSpace(x.DeedNumber));

            RuleFor(x => x.MunicipalityNumber)
                .MaximumLength(100).WithMessage("رقم البلدية يجب ألا يتجاوز 100 حرف")
                .When(x => !string.IsNullOrWhiteSpace(x.MunicipalityNumber));

            RuleFor(x => x.TotalFloors)
                .GreaterThan(0).WithMessage("عدد الطوابق يجب أن يكون أكبر من صفر")
                .When(x => x.TotalFloors.HasValue);

            RuleFor(x => x.UnitsCount)
                .GreaterThanOrEqualTo(0).WithMessage("عدد الوحدات يجب ألا يكون سالباً")
                .When(x => x.UnitsCount.HasValue);
        }
    }
}
