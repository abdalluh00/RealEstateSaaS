using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Properties.Offices.Commands.UpdateOffice
{
    public class UpdateOfficeCommandValidator : AbstractValidator<UpdateOfficeCommand>
    {
        public UpdateOfficeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المكتب مطلوب");

            RuleFor(x => x.PropertyCode)
                .MaximumLength(50);

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("عنوان العقار مطلوب")
                .MaximumLength(300);

            RuleFor(x => x.Description)
                .MaximumLength(2000)
                .When(x => !string.IsNullOrWhiteSpace(x.Description));

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

            RuleFor(x => x.FacingDirection)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.FacingDirection));

            RuleFor(x => x.RegaLicenseNumber)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.RegaLicenseNumber));

            RuleFor(x => x.DeedNumber)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.DeedNumber));

            RuleFor(x => x.MunicipalityNumber)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.MunicipalityNumber));

            RuleFor(x => x.ParkingSpots)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ParkingSpots.HasValue);

            RuleFor(x => x.AgeInYears)
                .GreaterThanOrEqualTo(0)
                .When(x => x.AgeInYears.HasValue);

            RuleFor(x => x.UnitNumber)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.UnitNumber));

            RuleFor(x => x.Floor)
                .GreaterThanOrEqualTo(0).WithMessage("رقم الطابق يجب أن يكون صفر أو أكبر");

            RuleFor(x => x.Bathrooms)
                .GreaterThanOrEqualTo(0).WithMessage("عدد دورات المياه لا يمكن أن يكون سالباً");

            RuleFor(x => x.OfficesCount)
                .GreaterThan(0).WithMessage("عدد المكاتب يجب أن يكون أكبر من صفر");

            RuleFor(x => x.MeetingRooms)
                .GreaterThanOrEqualTo(0)
                .When(x => x.MeetingRooms.HasValue);

            RuleFor(x => x.OwnerId)
                .Must(x => x == null || x != Guid.Empty)
                .WithMessage("معرف المالك غير صالح");

            RuleFor(x => x.AgentId)
                .Must(x => x == null || x != Guid.Empty)
                .WithMessage("معرف الوسيط غير صالح");

            RuleFor(x => x.ParentPropertyId)
                .Must(x => x == null || x != Guid.Empty)
                .WithMessage("معرف العقار الأب غير صالح");
        }
    }
}
