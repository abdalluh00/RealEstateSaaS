using FluentValidation;

namespace RealEstate.Application.Features.Appointments.Commands.CreateAppointment
{
    public sealed class CreateAppointmentCommandValidator
        : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("العقار مطلوب");

            RuleFor(x => x.ClientId)
                .NotEmpty().WithMessage("العميل مطلوب");

            RuleFor(x => x.AgentId)
                .NotEmpty().WithMessage("الوكيل مطلوب");

            RuleFor(x => x.ScheduledAt)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("تاريخ الموعد يجب أن يكون في المستقبل");

            RuleFor(x => x.DurationMinutes)
                .InclusiveBetween(15, 180)
                .WithMessage("مدة الموعد يجب أن تكون بين 15 و 180 دقيقة");
        }
    }
}