using MediatR;
using RealEstate.Domain.Common.Enums;
using RealEstate.Shared.Common;
namespace RealEstate.Application.Features.PropertyMedia.Commands.UploadMedia
{
    public record UploadMediaCommand(
        Guid PropertyId,
        string FileName,
        string ContentType,
        Stream FileStream,
        MediaType MediaType,  // Image, Video, Document
        bool IsCover,
        int SortOrder
    ) : IRequest<ApiResponse<Guid>>
    {
        

        public Stream? Stream { get; }
        public Microsoft.AspNetCore.Mvc.Formatters.MediaType MediaType1 { get; }
    }
}
