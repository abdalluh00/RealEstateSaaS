using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Appointments.Commands.UpdateAppointment
{
    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentCommand>
    {
        public UpdateAppointmentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.PropertyId).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.AgentId).NotEmpty();

            RuleFor(x => x.ScheduledAt)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("تاريخ الموعد يجب أن يكون في المستقبل");
        }
    }
}
