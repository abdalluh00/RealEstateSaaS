using FluentValidation;
namespace RealEstate.Application.Features.Properties.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المستودع مطلوب");

            RuleFor(x => x).NotNull();

            When(x => x != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("عنوان المستودع مطلوب")
                    .MaximumLength(300);

                RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر");

                RuleFor(x => x.Area)
                    .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر");

                RuleFor(x => x.City)
                    .NotEmpty().WithMessage("المدينة مطلوبة")
                    .MaximumLength(100);

                RuleFor(x => x.District)
                    .NotEmpty().WithMessage("الحي مطلوب")
                    .MaximumLength(100);

                RuleFor(x => x.Address)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.Address));

              
                

                RuleFor(x => x.RegaLicenseNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.RegaLicenseNumber));

                RuleFor(x => x.DeedNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.DeedNumber));

                RuleFor(x => x.MunicipalityNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.MunicipalityNumber));

                RuleFor(x => x.OwnerId)
                    .NotEmpty().WithMessage("المالك مطلوب");

                RuleFor(x => x.AgentId)
                    .NotEmpty().WithMessage("الوسيط/الموظف المسؤول مطلوب");

                RuleFor(x => x.CeilingHeight)
                    .GreaterThan(0)
                    .When(x => x.CeilingHeight.HasValue)
                    .WithMessage("ارتفاع السقف يجب أن يكون أكبر من صفر");

                RuleFor(x => x.LoadingDocks)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.LoadingDocks.HasValue)
                    .WithMessage("عدد منصات التحميل لا يمكن أن يكون سالباً");

               
            });
        }
    }
}
