using FluentValidation;
namespace RealEstate.Application.Features.Companies.Commands.CreateCompany
{
    public class CreateCompanyValidator : AbstractValidator<CreateCompanyCommand>
    {
        private static readonly string[] AllowedPlans = ["Basic", "Pro", "Business"];

        public CreateCompanyValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("اسم الشركة مطلوب")
                .MaximumLength(200);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الجوال مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            RuleFor(x => x.SubscriptionPlan)
                .NotEmpty()
                .Must(p => AllowedPlans.Contains(p))
                .WithMessage("الباقة يجب أن تكون Basic أو Pro أو Business");

            RuleFor(x => x.SubscriptionExpiry)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("تاريخ انتهاء الاشتراك يجب أن يكون في المستقبل");
        }
    }
}
