using FluentValidation;

namespace RealEstate.Application.Features.Maintenance.Commands.UploadMaintenanceMedia
{
    public sealed class UploadMaintenanceMediaCommandValidator
        : AbstractValidator<UploadMaintenanceMediaCommand>
    {
        private static readonly string[] AllowedExtensions =
            [".jpg", ".jpeg", ".png", ".webp", ".mp4"];

        private const long MaxBytes = 20 * 1024 * 1024;

        public UploadMaintenanceMediaCommandValidator()
        {
            RuleFor(x => x.MaintenanceRequestId)
                .NotEmpty().WithMessage("معرف طلب الصيانة مطلوب");

            RuleFor(x => x.File)
                .NotNull().WithMessage("الملف مطلوب")
                .Must(f => f is not null &&
                    AllowedExtensions.Contains(
                        Path.GetExtension(f.FileName).ToLowerInvariant()))
                .WithMessage("نوع الملف غير مسموح")
                .Must(f => f is not null && f.Length <= MaxBytes)
                .WithMessage("حجم الملف يتجاوز الحد المسموح 20MB");

            RuleFor(x => x.Stage)
                .IsInEnum().WithMessage("مرحلة الصورة غير صحيحة");

            RuleFor(x => x.UploadedById)
                .NotEmpty().WithMessage("معرف من رفع الملف مطلوب");
        }
    }
}