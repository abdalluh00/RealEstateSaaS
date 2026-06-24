using FluentValidation;

namespace RealEstate.Application.Features.Clients.Commands.CreateClient
{
    public class CreateClientValidator : AbstractValidator<CreateClientCommand>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("اسم العميل مطلوب")
                .MaximumLength(200);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الجوال مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.CompanyId)
                .NotEmpty().WithMessage("معرف الشركة مطلوب");
        }
    }
}
