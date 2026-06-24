using FluentValidation;

namespace RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia
{
    public class UploadMediaValidator : AbstractValidator<UploadMediaCommand>
    {
        private static readonly string[] AllowedTypes = ["Image", "Video", "Document"];
        private static readonly string[] AllowedImageExts = [".jpg", ".jpeg", ".png", ".webp"];
        private static readonly string[] AllowedVideoExts = [".mp4", ".mov"];

        public UploadMediaValidator()
        {
            RuleFor(x => x.PropertyId).NotEmpty();

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("اسم الملف مطلوب");

            RuleFor(x => x.MediaType)
                .NotEmpty()
                .WithMessage("نوع الملف يجب أن يكون Image أو Video أو Document");

            RuleFor(x => x.FileStream)
                .NotNull().WithMessage("الملف مطلوب");
        }
    }
}
