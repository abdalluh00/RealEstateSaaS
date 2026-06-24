using FluentValidation;
namespace RealEstate.Application.Features.Appointments.Commands.UpdateAppointmentStatus
{
    public class UpdateAppointmentStatusValidator : AbstractValidator<UpdateAppointmentStatusCommand>
    {
        public UpdateAppointmentStatusValidator()
        {
            RuleFor(x => x.Id).NotEmpty();

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage("حالة الموعد غير صحيحة");
        }
    }
}
