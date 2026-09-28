using FluentValidation;
using RealEstate.Domain.Common.Enums;

namespace RealEstate.Application.Features.Users.Commands.InviteUser
{
    public sealed class InviteUserCommandValidator
        : AbstractValidator<InviteUserCommand>
    {
        public InviteUserCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("الاسم مطلوب")
                .MaximumLength(200).WithMessage("الاسم لا يتجاوز 200 حرف");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("البريد الإلكتروني مطلوب")
                .EmailAddress().WithMessage("البريد الإلكتروني غير صحيح");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("رقم الجوال مطلوب")
                .Matches(@"^05\d{8}$").WithMessage("رقم الجوال غير صحيح");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("الدور غير صحيح")
                .Must(r => r != UserRole.Owner)
                .WithMessage("لا يمكن دعوة مالك آخر");
        }
    }
}