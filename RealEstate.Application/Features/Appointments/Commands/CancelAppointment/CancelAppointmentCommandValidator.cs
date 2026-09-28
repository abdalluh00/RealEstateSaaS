using FluentValidation;

namespace RealEstate.Application.Features.Appointments.Commands.CancelAppointment
{
    public sealed class CancelAppointmentCommandValidator
        : AbstractValidator<CancelAppointmentCommand>
    {
        public CancelAppointmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الموعد مطلوب");

            RuleFor(x => x.CancellationReason)
                .NotEmpty().WithMessage("سبب الإلغاء مطلوب")
                .MaximumLength(500).WithMessage("سبب الإلغاء لا يتجاوز 500 حرف");
        }
    }
}