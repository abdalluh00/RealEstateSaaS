using FluentValidation;

namespace RealEstate.Application.Features.Clients.Commands.UpdateClient
{
    public sealed class UpdateClientCommandValidator
        : AbstractValidator<UpdateClientCommand>
    {
        public UpdateClientCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف العميل مطلوب");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("اسم العميل مطلوب")
                .MaximumLength(200).WithMessage("الاسم لا يتجاوز 200 حرف");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الجوال مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => x.Email is not null)
                .WithMessage("البريد الإلكتروني غير صحيح");
        }
    }
}