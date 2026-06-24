using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Warehouses.Commands.UpdateWarehouse
{
    public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
    {
        public UpdateWarehouseCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المستودع مطلوب");

            RuleFor(x => x.Warehouse).NotNull();

            When(x => x.Warehouse != null, () =>
            {
                RuleFor(x => x.Warehouse.Title)
                    .NotEmpty().WithMessage("عنوان المستودع مطلوب")
                    .MaximumLength(300);

                RuleFor(x => x.Warehouse.Price)
                    .GreaterThan(0).WithMessage("السعر يجب أن يكون أكبر من صفر");

                RuleFor(x => x.Warehouse.Area)
                    .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من صفر");

                RuleFor(x => x.Warehouse.City)
                    .NotEmpty().WithMessage("المدينة مطلوبة")
                    .MaximumLength(100);

                RuleFor(x => x.Warehouse.District)
                    .NotEmpty().WithMessage("الحي مطلوب")
                    .MaximumLength(100);

                RuleFor(x => x.Warehouse.Address)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.Warehouse.Address));

                RuleFor(x => x.Warehouse.FacingDirection)
                    .MaximumLength(50)
                    .When(x => !string.IsNullOrWhiteSpace(x.Warehouse.FacingDirection));

                RuleFor(x => x.Warehouse.RegaLicenseNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Warehouse.RegaLicenseNumber));

                RuleFor(x => x.Warehouse.DeedNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Warehouse.DeedNumber));

                RuleFor(x => x.Warehouse.MunicipalityNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Warehouse.MunicipalityNumber));

                RuleFor(x => x.Warehouse.OwnerId)
                    .NotEmpty().WithMessage("المالك مطلوب");

                RuleFor(x => x.Warehouse.AgentId)
                    .NotEmpty().WithMessage("الوسيط/الموظف المسؤول مطلوب");

                RuleFor(x => x.Warehouse.CeilingHeight)
                    .GreaterThan(0)
                    .When(x => x.Warehouse.CeilingHeight.HasValue)
                    .WithMessage("ارتفاع السقف يجب أن يكون أكبر من صفر");

                RuleFor(x => x.Warehouse.LoadingDocks)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Warehouse.LoadingDocks.HasValue)
                    .WithMessage("عدد منصات التحميل لا يمكن أن يكون سالباً");

                RuleFor(x => x.Warehouse.ElectricityCapacity)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Warehouse.ElectricityCapacity));
            });
        }
    }
}
