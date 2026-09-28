using FluentValidation;

namespace RealEstate.Application.Features.Clients.Commands.UpdateLeadStatus
{
    public sealed class UpdateLeadStatusCommandValidator
        : AbstractValidator<UpdateLeadStatusCommand>
    {
        public UpdateLeadStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف العميل مطلوب");

            RuleFor(x => x.LeadStatus)
                .IsInEnum().WithMessage("حالة العميل غير صحيحة");
        }
    }
}