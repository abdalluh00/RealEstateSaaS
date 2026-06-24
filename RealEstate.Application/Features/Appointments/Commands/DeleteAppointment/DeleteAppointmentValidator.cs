using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Application.Features.Appointments.Commands.DeleteAppointment
{
    public class DeleteAppointmentValidator : AbstractValidator<DeleteAppointmentCommand>
    {
        public DeleteAppointmentValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
