using FluentValidation;

namespace RealEstate.Application.Features.Appointments.Commands.CompleteAppointment
{
    public sealed class CompleteAppointmentCommandValidator
        : AbstractValidator<CompleteAppointmentCommand>
    {
        public CompleteAppointmentCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("معرف الموعد مطلوب");

            RuleFor(x => x.ActualVisitAt)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("وقت الزيارة الفعلي لا يمكن أن يكون في المستقبل");

            RuleFor(x => x.Result)
                .IsInEnum().WithMessage("نتيجة الزيارة غير صحيحة");
        }
    }
}