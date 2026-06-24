using FluentValidation;

namespace RealEstate.Application.Features.Appointments.Commands.CreateAppointment
{
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.AgentId).NotEmpty();

            RuleFor(x => x.ScheduledAt)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("تاريخ الموعد يجب أن يكون في المستقبل");
        }
    }
}
