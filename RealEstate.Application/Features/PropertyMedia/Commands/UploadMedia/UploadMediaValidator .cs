using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia
{
    public sealed class UploadPropertyMediaCommandValidator
        : AbstractValidator<UploadPropertyMediaCommand>
    {
        private static readonly string[] AllowedExtensions =
            [".jpg", ".jpeg", ".png", ".webp", ".mp4"];

        private const long MaxBytes = 20 * 1024 * 1024; // 20MB

        public UploadPropertyMediaCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("معرف العقار مطلوب");

            RuleFor(x => x.File)
                .NotNull().WithMessage("الملف مطلوب")
                .Must(BeValidExtension)
                .WithMessage("نوع الملف غير مسموح — jpg, jpeg, png, webp, mp4 فقط")
                .Must(BeValidSize)
                .WithMessage("حجم الملف يتجاوز الحد المسموح 20MB");
        }

        private static bool BeValidExtension(IFormFile? file)
        {
            if (file is null) return false;
            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            return AllowedExtensions.Contains(ext);
        }

        private static bool BeValidSize(IFormFile? file) =>
            file is not null && file.Length <= MaxBytes;
    }
}