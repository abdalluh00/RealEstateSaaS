using FluentValidation;
using RealEstate.Application.Features.Properties.Apartments.Commands.UpdateApartment;

namespace RealEstate.Application.Features.Apartments.Commands.UpdateApartment
{
    public class UpdateApartmentValidator : AbstractValidator<UpdateApartmentCommand>
    {
        public UpdateApartmentValidator()
        {
            RuleFor(x => x.ApartmentId)
                .NotEmpty().WithMessage("معرف الشقة مطلوب");

            RuleFor(x => x.Apartment).NotNull();

            When(x => x.Apartment is not null, () =>
            {
                RuleFor(x => x.Apartment.Title)
                    .NotEmpty().WithMessage("عنوان العقار مطلوب")
                    .MaximumLength(300);

                RuleFor(x => x.Apartment.City)
                    .NotEmpty().WithMessage("المدينة مطلوبة")
                    .MaximumLength(100);

                RuleFor(x => x.Apartment.District)
                    .NotEmpty().WithMessage("الحي مطلوب")
                    .MaximumLength(100);

                RuleFor(x => x.Apartment.Address)
                    .MaximumLength(500)
                    .When(x => !string.IsNullOrWhiteSpace(x.Apartment.Address));

                RuleFor(x => x.Apartment.FacingDirection)
                    .MaximumLength(50)
                    .When(x => !string.IsNullOrWhiteSpace(x.Apartment.FacingDirection));

                RuleFor(x => x.Apartment.RegaLicenseNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Apartment.RegaLicenseNumber));

                RuleFor(x => x.Apartment.DeedNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Apartment.DeedNumber));

                RuleFor(x => x.Apartment.MunicipalityNumber)
                    .MaximumLength(100)
                    .When(x => !string.IsNullOrWhiteSpace(x.Apartment.MunicipalityNumber));

                RuleFor(x => x.Apartment.Price)
                    .GreaterThanOrEqualTo(0).WithMessage("السعر يجب أن يكون أكبر من أو يساوي 0");

                RuleFor(x => x.Apartment.Area)
                    .GreaterThan(0).WithMessage("المساحة يجب أن تكون أكبر من 0");

                RuleFor(x => x.Apartment.OwnerId)
                    .NotEmpty().WithMessage("المالك مطلوب");

                RuleFor(x => x.Apartment.AgentId)
                    .NotEmpty().WithMessage("الوسيط/الموظف المسؤول مطلوب");

                RuleFor(x => x.Apartment.UnitNumber)
                    .NotEmpty().WithMessage("رقم الوحدة مطلوب")
                    .MaximumLength(50);

                RuleFor(x => x.Apartment.Bedrooms)
                    .GreaterThanOrEqualTo(0);

                RuleFor(x => x.Apartment.Bathrooms)
                    .GreaterThanOrEqualTo(0);

                RuleFor(x => x.Apartment.FloorNumber)
                    .GreaterThanOrEqualTo(0);

                RuleFor(x => x.Apartment.LivingRooms)
                    .GreaterThanOrEqualTo(0)
                    .When(x => x.Apartment.LivingRooms.HasValue);
            });
        }
    }
}