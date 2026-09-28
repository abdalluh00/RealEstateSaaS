using FluentValidation;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Owners.Commands.UpdateOwner
{
    public sealed class UpdateOwnerCommandValidator
        : AbstractValidator<UpdateOwnerCommand>
    {
        public UpdateOwnerCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف المالك مطلوب");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("اسم المالك مطلوب")
                .MaximumLength(200).WithMessage("الاسم لا يتجاوز 200 حرف");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الجوال مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => x.Email is not null)
                .WithMessage("البريد الإلكتروني غير صحيح");

            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("اسم الشركة مطلوب عند اختيار نوع الشركة")
                .When(x => x.OwnerType == OwnerType.Company);

            RuleFor(x => x.CommissionRate)
                .InclusiveBetween(0, 100)
                .When(x => x.CommissionRate.HasValue)
                .WithMessage("نسبة العمولة يجب أن تكون بين 0 و 100");

            RuleFor(x => x.IBAN)
                .Matches(@"^SA\d{22}$")
                .When(x => x.IBAN is not null)
                .WithMessage("رقم الآيبان غير صحيح");
        }
    }
}