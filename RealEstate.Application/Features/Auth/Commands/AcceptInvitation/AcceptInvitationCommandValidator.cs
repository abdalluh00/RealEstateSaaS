using FluentValidation;

namespace RealEstate.Application.Features.Auth.Commands.AcceptInvitation
{
    public sealed class AcceptInvitationCommandValidator
        : AbstractValidator<AcceptInvitationCommand>
    {
        public AcceptInvitationCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("رمز الدعوة مطلوب");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("كلمة المرور مطلوبة")
                .MinimumLength(8).WithMessage("كلمة المرور يجب أن تكون 8 أحرف على الأقل");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("كلمات المرور غير متطابقة");
        }
    }
}