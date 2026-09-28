using FluentValidation;

namespace RealEstate.Application.Features.PropertyDocuments.Commands.UploadDocument
{
    public sealed class UploadPropertyDocumentCommandValidator
        : AbstractValidator<UploadPropertyDocumentCommand>
    {
        private static readonly string[] AllowedExtensions =
            [".pdf", ".jpg", ".jpeg", ".png"];

        private const long MaxBytes = 10 * 1024 * 1024; // 10MB

        public UploadPropertyDocumentCommandValidator()
        {
            RuleFor(x => x.PropertyId)
                .NotEmpty().WithMessage("معرف العقار مطلوب");

            RuleFor(x => x.DocumentName)
                .NotEmpty().WithMessage("اسم المستند مطلوب")
                .MaximumLength(200).WithMessage("الاسم لا يتجاوز 200 حرف");

            RuleFor(x => x.DocumentType)
                .IsInEnum().WithMessage("نوع المستند غير صحيح");

            RuleFor(x => x.File)
                .NotNull().WithMessage("الملف مطلوب")
                .Must(f => f is not null &&
                    AllowedExtensions.Contains(
                        Path.GetExtension(f.FileName).ToLowerInvariant()))
                .WithMessage("نوع الملف غير مسموح — pdf, jpg, jpeg, png فقط")
                .Must(f => f is not null && f.Length <= MaxBytes)
                .WithMessage("حجم الملف يتجاوز الحد المسموح 10MB");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(x => x.IssueDate)
                .When(x => x.IssueDate.HasValue && x.ExpiryDate.HasValue)
                .WithMessage("تاريخ الانتهاء يجب أن يكون بعد تاريخ الإصدار");
        }
    }
}